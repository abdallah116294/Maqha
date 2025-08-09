using Maqha.Core.Services;
using Maqha.Utilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Generatetoke([FromBody]LoginDTO loginDTO) 
        {
            var result = await _authService.GenerateToken(loginDTO);
            return CreateResponse(result);
        }
    }
}
