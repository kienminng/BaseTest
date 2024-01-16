using Azure;
using BaseTest.Models.Entities;
using BaseTest.Models.ReponseModels.UserCard;
using BaseTest.Models.RequestModels.UserCard;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Businiss.IService
{
    public interface IUserCardService
    {
        Task<TokenModel> Login([NotNull] LoginRequest request);
        Task Register(RegisterRequest register, string role);
        Task Update(UserUpdateFormRequest request);
        IQueryable<UserCard> GetPageUser(GetPageUserInput input);
        Task<UserProfileResponse> GetLoginUser();
        Task<UserProfileResponse> GetById(int id);
        Task ActiveUser(string token);
        Task ChangeStatus(int id);
        Task Update(UserCard userCard);
        Task SendResetPassword(string usernameOrEmail);
        Task ResetPassword(ResetPasswordRequest request);
        Task<string> GenericAccessTokenByRefreshToken(string refreshToken);
        Task ChangePassword(ChangePasswordRequest request);
    }
}
