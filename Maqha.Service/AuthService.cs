using Maqha.Core.IRepository;
using Maqha.Core.Services;
using Maqha.Utilities.DTO;
using Maqha.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Service
{
    public class AuthService : IAuthService
    {
        private readonly TokenHelper _tokenHelper;
        private readonly IUserRepository _userRepository;

        public AuthService(TokenHelper tokenHelper, IUserRepository userRepository)
        {
            _tokenHelper = tokenHelper;
            _userRepository = userRepository;
        }

        public async Task<ResponseDTO<object>> GenerateToken(LoginDTO loginDTo)
        {
            try
            {
                var checkuser = await _userRepository.CheckUserAsync(loginDTo.Email);
                if (!string.IsNullOrEmpty(checkuser.id))
                {
                    var user = await _userRepository.CheckUserWithPasswordAsync(loginDTo);
                    if (user.Succeeded)
                    {
                        var tokenDto = new TokenDTO
                        {
                            Email = loginDTo.Email,
                            Id = checkuser.id,
                            Role = checkuser.Role,
                        };
                        string token = _tokenHelper.GenerateToken(tokenDto);
                        if (!string.IsNullOrEmpty(token))
                        {
                            return new ResponseDTO<object>
                            {
                                IsSuccess=true,
                                Data = new {token=token},
                                Message="Token Genereted"
                            };
                        }
                    }
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = "Token not Generated",
                        ErrorCode = ErrorCodes.BadRequest,
                    };
                }
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Token not Generated",
                    ErrorCode = ErrorCodes.BadRequest,
                };
            }
            catch (Exception)
            {
                return new ResponseDTO<object> { IsSuccess = false, Data = null, Message = "Exception Occured ", ErrorCode = ErrorCodes.Exception };
            }
        }
    }
}
