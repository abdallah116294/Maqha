using Maqha.Core.Services;
using Maqha.Utilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController(IRoleService _roleService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            return CreateResponse(result);
        }
        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string UserEmail)
        {
            var result = await _roleService.GetUserRoles(UserEmail);
            return CreateResponse(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDTO dto)
        {
            var role = await _roleService.CreateRoleAsync(dto.RoleName);
            return CreateResponse(role);
        }
        [HttpPost("add-role-to-user")]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserDTO dto)
        {
            var result = await _roleService.AddRoleToUserAsync(dto.Email, dto.RoleName);
            return CreateResponse(result);

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string RoleName)
        {
            var result = await _roleService.RemoveRoleAsync(RoleName);
            return CreateResponse(result);
        }
    }

}
