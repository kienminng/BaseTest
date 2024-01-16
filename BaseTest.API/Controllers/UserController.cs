using System.Diagnostics.CodeAnalysis;
using BaseTest.Businiss.IService;
using BaseTest.Common;
using BaseTest.Models.Entities;
using BaseTest.Models.ReponseModels.UserCard;
using BaseTest.Models.RequestModels.UserCard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseTest.API.Controllers
{
    [Route(Constant.DefaultRouter)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserCardService _userService;

        public UserController(IUserCardService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest register)
        {
            await _userService.Register(register,Constant.Roles.RoleUser);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginRequest login)
        {
            var result = await _userService.Login(login);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UserUpdateFormRequest request)
        {
            await _userService.Update(request);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Constant.Roles.RoleAdmin)]
        public async Task<IActionResult> CreateAdmin([FromForm] RegisterRequest request)
        {
             await _userService.Register(request, Constant.Roles.RoleAdmin);
             return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Profile()
        {
            var result = await _userService.GetLoginUser();
            return Ok(result);
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> FindById(int userId)
        {
            var result = await _userService.GetById(userId);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = Constant.Roles.RoleAdmin)]
        public IActionResult GetPageUser([FromQuery] GetPageUserInput input)
        {
            var result = _userService.GetPageUser(input);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ActiveUser([FromQuery] string token)
        {
            await _userService.ActiveUser(token);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendResetPassword([FromForm][NotNull] string usernameOrEmail)
        {
            await _userService.SendResetPassword(usernameOrEmail);
            return Ok();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            await _userService.ResetPassword(request);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Constant.Roles.RoleAdmin)]
        public async Task<IActionResult> ChangeStatus([FromForm] int id)
        {
            await _userService.ChangeStatus(id);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAccessTokenByRefreshToken(string refreshToken)
        {
            await _userService.GenericAccessTokenByRefreshToken(refreshToken);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            await _userService.ChangePassword(request);
            return Ok();
        }

        
    }
}
