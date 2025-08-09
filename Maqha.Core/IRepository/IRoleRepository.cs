using Maqha.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.IRepository
{
    public interface IRoleRepository
    {
        // Get all roles
        Task<List<string>> GetAllRolesAsync();
        // Create a role
        Task<bool> CreateRoleAsync(string roleName);
        // Add a role to a user
        Task<bool> AddRoleToUserAsync(string email, string roleName);
        // Remove a role
        Task<bool> RemoveRoleAsync(string roleName);
        //Get the User Roles by Email
        Task<List<string>> GetUserRoles(string userEmail);
    }
}
