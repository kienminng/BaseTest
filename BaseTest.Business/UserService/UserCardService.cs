using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using BaseTest.Businiss.IService;
using BaseTest.Businiss.MailService;
using BaseTest.Common;
using BaseTest.Models.Entities;
using BaseTest.Models.Form;
using BaseTest.Models.ReponseModels.UserCard;
using BaseTest.Models.RequestModels.UserCard;
using BaseTest.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;

namespace BaseTest.Businiss.Service;

public class UserCardService : IUserCardService
{
    private readonly ITokenService _tokenService;
    private readonly IBaseRepository<UserCard> _userCardRepo;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IMailService _mailService;
    private readonly IBaseRepository<Token> _tokenRepo;
    private readonly IBaseRepository<Roles> _roleRepo;

    public UserCardService(IBaseRepository<UserCard> userCardRepository, ITokenService tokenService,
        IHttpContextAccessor http, IMailService mailService , IBaseRepository<Token> tokenRepo , IBaseRepository<Roles> roleRepo)
    {
        _userCardRepo = userCardRepository;
        _tokenService = tokenService;
        _httpContext = http;
        _mailService = mailService;
        _tokenRepo = tokenRepo;
        _roleRepo = roleRepo;
    }

    public async Task<TokenModel> Login([NotNull] LoginRequest request)
    {
        var user = await _userCardRepo.GetAsync(opt =>
            (opt.Username.ToLower().Equals(request.Username.ToLower()) || opt.Email.ToLower().Equals(request.Username))
            && opt.Password.ToLower().Equals(EndCoding.Base64(request.Password.ToLower()))
            && opt.IsActive == true
            && !opt.IsDeleted
        );

        if (user == null) throw new Exception(ErrorMessage.UserErrorMessage.UserNotFound);
        if (user.Status == false) throw new Exception(ErrorMessage.UserErrorMessage.UserWasBand);
        var role = await _roleRepo.GetAsync(record => record.Id == user.RoleId);
        var claims = CreateClaims(user, role);

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var userToken = await _tokenRepo.GetAsync(record => record.UserId == user.Id);

        if (userToken == null)
        {
            var token = new Token()
            {
                RefreshToken = refreshToken,
                ExpiredDate = DateTime.UtcNow.AddDays(15),
                UpdateAt = DateTime.UtcNow
            };
            user.Token = token;
        }
        else
        {
            userToken.RefreshToken = refreshToken;
            userToken.ExpiredDate = DateTime.UtcNow.AddDays(15);
            userToken.UpdateAt = DateTime.UtcNow;
            user.Token = userToken;
        }
        
        try
        {
            await _userCardRepo.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _userCardRepo.ClearTrackedChanges();
            throw;
        }

        return new TokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task Register(RegisterRequest request, string roles)
    {
        if (!await Validate(request)) throw new Exception();
        var role = await _roleRepo.GetAsync(record => record.Name.ToLower().Equals(roles.ToLower()));

        var user = new UserCard
        {
            Username = request.Username,
            Password = EndCoding.Base64(request.Password.ToLower()),
            Email = request.Email,
            Roles = role,
            PhoneNumber = request.PhoneNumber,
            Status = true,
            IsActive = false
        };
        try
        {
            var data = await _userCardRepo.CreateAsync(user);
            var claims = CreateClaims(data, data.Roles);
            var token = _tokenService.GenerateAccessToken(claims);
            var url = $"{Constant.Url.ConfirmMail}?token={token}";
            MailForm mailForm = new MailForm()
            {
                To = data.Email,
                Subject = "Welcome join with Us",
                Body = $"<h1> Hello {data.Username} </h1>" +
                       "<h2> Please click the button to active your user account</h2>" +
                       $"<a href=\"{url}\">{Constant.Html.Button}</a>" +
                       "<h3> This is confidential information. </h3>" +
                       "<h3> Do not share it with anyone </h3>"
            };
            _mailService.SendMail(mailForm);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            _userCardRepo.ClearTrackedChanges();
            throw;
        }
    }

    public async Task Update(UserUpdateFormRequest request)
    {
        var loginUser = await GetLoginUser();
        if (await IsExistEmail(request.Email, loginUser.Id)) throw new Exception(ErrorMessage.UserErrorMessage.EmailIsExisted);
        var user = new UserCard()
        {
            Id = loginUser.Id,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };

        try
        {
            await _userCardRepo.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _userCardRepo.ClearTrackedChanges();
            throw;
        }
    } 

    public IQueryable<UserCard> GetPageUser(GetPageUserInput input)
    {
        var query = _userCardRepo.GetQueryable(record => !record.IsDeleted);
        query = ApplySearchFilter(query, input);
        query = BaseApplyPaging.ApplyPaging(query, input.PageSize, input.PageNo, out var totalItem);
        return query;
    }

    public async Task<UserProfileResponse> GetLoginUser()
    {
        var username = (_httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value!);
        var user = await _userCardRepo.GetAsync(record =>
            !record.IsDeleted && record.Username.ToLower().Equals(username.ToLower()));
        var data = new UserProfileResponse()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Roles = user.Roles,
            Amount = user.Amount,
            createDate = user.CreateAt,
            PhoneNumber = user.PhoneNumber,
        };
        return data;
    }


    public async Task<UserProfileResponse> GetById(int id)
    {
        var user = await _userCardRepo.GetByIDAsync(id);

        var data = new UserProfileResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Roles = user.Roles,
            Amount = user.Amount,
            createDate = user.CreateAt,
            PhoneNumber = user.PhoneNumber,
        };
        return data;
    }

    public async Task ActiveUser(string token)
    {
        var claims = _tokenService.GetPrincipalFromExpiredToken(token);
        var id = int.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value!);
        var user = await UserIsExisted(id);

        user.IsActive = true;

        try
        {
            await _userCardRepo.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _userCardRepo.ClearTrackedChanges();
            throw;
        }
    }

    public async Task ChangeStatus(int id)
    {
        var user = await UserIsExisted(id);

        user.Status = !user.Status;
        try
        {
            await _userCardRepo.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _userCardRepo.ClearTrackedChanges();
            throw;
        }
    }

    public async Task Update(UserCard userCard)
    {
        var user = await UserIsExisted(userCard.Id);
        user = userCard;
        user.UpdateAt = DateTime.UtcNow;

        try
        {
            await _userCardRepo.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _userCardRepo.ClearTrackedChanges();
            throw;
        }
    }

    public async Task SendResetPassword(string usernameOrEmail)
    {
        var user = await _userCardRepo.GetAsync(record =>
            !record.IsDeleted && (record.Username == usernameOrEmail || record.Email == usernameOrEmail));
        if (user is null)
        {
            throw new Exception(ErrorMessage.UserErrorMessage.UserNotFound);
        }

        var str = EndCoding.GenerateRandomString(10);
        user.ResetPasswordToken = str;
        user.ResetPasswordTokenExpiresDate = DateTime.Now.AddSeconds(30);
        MailForm mailForm = new MailForm()
        {
            To = user.Email,
            Subject = "Your Reset password Token",
            Body = $"<h1> Hello {user.Username} </h1>" +
                   $"<h2> This is your reset password Key: {str}</h2>" +
                   "<h3> After 30 seconds the code will be disabled </h3>" +
                   "<h3> This is confidential information. </h3>" +
                   "<h3> Do not share it with anyone </h3>"
        };
        _mailService.SendMail(mailForm);
    }

    public async Task ResetPassword([NotNull] ResetPasswordRequest request)
    {
        var user = await _userCardRepo.GetAsync(record =>
            record.Username.ToLower().Equals(request.Username.ToLower())
            && record.ResetPasswordToken.ToLower().Equals(request.Code.ToLower())
        );
        if (user is null) throw new Exception(ErrorMessage.UserErrorMessage.UserNotFound);
        user.Password = EndCoding.Base64(request.NewPassword);

        try
        {
            await _userCardRepo.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _userCardRepo.ClearTrackedChanges();
            throw;
        }
    }

    public async Task ChangePassword(ChangePasswordRequest request)
    {
        int id = int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _userCardRepo.GetByIDAsync(id);

        var hashPassword = EndCoding.Base64(request.OldPassword.ToLower());
        if (!user.Password.Equals(hashPassword))
        {
            throw new Exception(ErrorMessage.UserErrorMessage.WrongPassword);
        }

        user.Password = EndCoding.Base64(request.OldPassword.ToLower());

        try
        {
            await _userCardRepo.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _userCardRepo.ClearTrackedChanges();
            throw;
        }
    }

    public async Task<string> GenericAccessTokenByRefreshToken(string refreshToken)
    {
        var user = await _userCardRepo.GetAsync(record =>
            record.Token.RefreshToken.ToLower().Equals(refreshToken.ToLower()) && !record.IsDeleted);

        if (user is null) throw new Exception(ErrorMessage.UserErrorMessage.UserNotFound);

        var claims = CreateClaims(user, user.Roles);
        var token = _tokenService.GenerateAccessToken(claims);
        return token;
    }

    private List<Claim> CreateClaims(UserCard user, Roles role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, role.Name),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        return claims;
    }

    private async Task<bool> Validate(RegisterRequest request)
    {
        try
        {
            var user = await _userCardRepo.GetAsync(opt => opt.Username.ToLower().Equals(request.Username.ToLower())
                                                           || opt.Email.ToLower().Equals(request.Email.ToLower())
            );
            if (user is null)
            {
                return true;
            }

            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    private async Task<UserCard> UserIsExisted(int id)
    {
        var user = await _userCardRepo.GetByIDAsync(id);
        if (user is null)
        {
            throw new Exception(ErrorMessage.UserErrorMessage.UserNotFound);
        }

        return user;
    }

    private async Task<bool> IsExistEmail(string email, int id)
    {
        var user = await _userCardRepo.GetAsync(record => record.Email.ToLower().Equals(email.ToLower())
                                                          && !record.IsDeleted
                                                          && record.Status 
                                                          && record.IsActive 
                                                          && record.Id != id
        );
        if (user is  null  )
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsExistEmail(string email)
    {
        var user = await _userCardRepo.GetAsync(record => !record.IsDeleted
                                                          && record.IsActive
                                                          && record.Status
                                                          && record.Email.ToLower().Equals(email.ToLower()));
        if (user is  null  )
        {
            return false;
        }

        return true;
    }

    private IQueryable<UserCard> ApplySearchFilter(IQueryable<UserCard> query, GetPageUserInput input)
    {
        if (input.role.IsNullOrEmpty()) query = query.Where(record => record.Roles.Name == input.role);

        if (!input.username.IsNullOrEmpty()) query = query.Where(record => record.Username.Contains(input.username));

        if (!input.email.IsNullOrEmpty()) query = query.Where(record => record.Email.Contains(input.email));

        return query;
    }
}