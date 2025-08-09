using Maqha.Core.Services;
using Maqha.Utilities.DTO;
using Maqha.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [EndpointDescription("create new user")]
       public async Task<IActionResult> Register(RegisterDTO registerDTO) 
        {
            var result = await _userService.Register(registerDTO);
            return CreateResponse(result);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult>login(LoginDTO loginDTO)
        {
            var result = await _userService.Login(loginDTO);
            return CreateResponse(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return CreateResponse<object>(new ResponseDTO<object> { 
                IsSuccess = true, 
                Message = "User Logged out successfully"
            });
        }
        [HttpPost("default-register")]
        [EndpointDescription("Create a new user record with default roles")]
        public async Task<IActionResult> RegisterDefault(RegisterDTO registerDto)
        {
            var result = await _userService.DefaultRegister(registerDto);
            return CreateResponse(result);
        }
        //[Authorize]
        //[AdminOnly]
        [HttpGet("check/{email}")]
        public async Task<IActionResult>CheckUser(string email)
        {
            var result=await _userService.CheckUser(email);
            return CreateResponse(result);
        }
        //[Authorize]
        //[AdminOnly]
        [HttpPost("check-with-password")]
        public async Task<IActionResult> CheckWithPassword([FromBody]LoginDTO loginDTO)
        {
            var result = await _userService.CheckUserWithPassword(loginDTO);
              return CreateResponse(result);
        }


    }
}
