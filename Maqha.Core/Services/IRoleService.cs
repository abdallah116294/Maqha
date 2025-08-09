using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maqha.Utilities.DTO;
namespace Maqha.Core.Services
{
    public interface IRoleService
    {
        //GetAll Rolles 
        Task<ResponseDTO<List<string>>> GetAllRolesAsync();
        //Create Role
        Task<ResponseDTO<object>>CreateRoleAsync(string roleName);
        //Add Role To User 
        Task<ResponseDTO<object>> AddRoleToUserAsync(string email, string roleName);
        // Remove Role
        Task<ResponseDTO<object>> RemoveRoleAsync( string roleName);
        Task<ResponseDTO<object>> GetUserRoles(string userEmail);

    }
}
