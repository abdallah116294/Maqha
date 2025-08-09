using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maqha.Utilities.DTO;
namespace Maqha.Core.IRepository
{
    public interface IUserRepository
    {
        // Method to register a user
        Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO);
        
        
        // Method to login a user
        Task<SignInResult> LoginAsync(LoginDTO loginDTo);
        
        // Method to logout a user
        Task LogoutAsync();
        
        // Check for user existence
        Task<UserDTo> CheckUserAsync(string email);
        
        // Check user with password
        Task<SignInResult> CheckUserWithPasswordAsync(LoginDTO loginDTo);
    }
}
