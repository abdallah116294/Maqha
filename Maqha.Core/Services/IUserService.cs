using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maqha.Utilities.DTO;
namespace Maqha.Core.Services
{
    public interface IUserService
    {
        // Method to register a user
        Task<ResponseDTO<object>> Register(RegisterDTO registerDTO);
        //Deualt Registration
        Task<ResponseDTO<object>> DefaultRegister(RegisterDTO dto);
        // Method to login a user
        Task<ResponseDTO<object>> Login(LoginDTO loginDTo);
        // Method to logout a user
        Task Logout();
        //Check For User Existence
        Task<ResponseDTO<UserDTo>> CheckUser(string email);
        //check user with Password
        Task<ResponseDTO<object>> CheckUserWithPassword(LoginDTO loginDTo);


    }
}
