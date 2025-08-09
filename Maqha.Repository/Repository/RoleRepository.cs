using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maqha.Core.IRepository;
using Maqha.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Maqha.Repository.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;
        public RoleRepository(UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole>roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> AddRoleToUserAsync(string email, string roleName)
        {
            //Find the user 
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            //Check if the role exists
            var existsRole = await _roleManager.RoleExistsAsync(roleName);
            if (!existsRole)
                return false;
            //Add the role to the user
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            //Check if the role name is valid
            if (string.IsNullOrWhiteSpace(roleName))
                return false;
            //Check if the role already exists
            var existsRole = await _roleManager.RoleExistsAsync(roleName);
            if(existsRole)
                return false;
            //Create the role
            var result = await _roleManager.CreateAsync(new AppIdentityRole(roleName));
            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            //get all roles from the database
            var role =await _roleManager.Roles.AsNoTracking().ToListAsync();
            if (role == null || !role.Any())
                return new List<string>();
            //return the role names as a list of strings
            return role.Select(r => r.Name).ToList();
        }

        public async Task<List<string>> GetUserRoles(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) 
                return null;
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return null;
            // Get the roles of the user
            var roles =await  _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> RemoveRoleAsync(string roleName)
        {
           //Find the role by name
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return false;
            //Remove the role
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return true;
            return false;
        }
    }
}
