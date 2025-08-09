using Maqha.Core.IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maqha.Utilities.DTO;
using Maqha.Repository.Identity;
using Maqha.Utilities.Helpers;
namespace Maqha.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
      
        //  private readonly SignInManager<IdentityUser> _signInManager;

        public UserRepository(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserDTo> CheckUserAsync(string email)
        {
            //find the user by email
            var user =await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                //return the user details as UserDTo
                return new UserDTo
                {
                    id = user.Id,
                    Email = user.Email,
                    Role = userRoles.FirstOrDefault() // Assuming a user can have only one role for simplicity
                };
            }
            return new UserDTo();

        }

        public async Task<SignInResult> CheckUserWithPasswordAsync(LoginDTO loginDTo)
        {
          var user =await _userManager.FindByEmailAsync(loginDTo.Email);
            if (user == null)
                return SignInResult.Failed;
            
            //Check the password
            return await _signInManager.CheckPasswordSignInAsync(user,loginDTo.Password, false);
        }

        public async Task<SignInResult> LoginAsync(LoginDTO loginDTo)
        {
            var user = await _userManager.FindByEmailAsync(loginDTo.Email);
            var result = await _signInManager.PasswordSignInAsync(user, loginDTo.Password, false,  false);
            return result;
        }

        public async Task LogoutAsync()
        {
             await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new AppIdentityUser
            {
                Name = registerDTO.Name,
                Surname = registerDTO.SurName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email
            };
            //Create the user with the provided password
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            return result;
        }
    }
}
