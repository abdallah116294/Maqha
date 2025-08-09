using FluentValidation;
using FluentValidation.Internal;
using Maqha.Core.IRepository;
using Maqha.Core.Services;
using Maqha.Utilities.DTO;
using Maqha.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Maqha.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly TokenHelper _tokenHelper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IValidator<RegisterDTO> registerValidator,TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _registerValidator = registerValidator;
            _tokenHelper = tokenHelper;
        }

        public async Task<ResponseDTO<UserDTo>> CheckUser(string email)
        {

            try
            {
                var result = await _userRepository.CheckUserAsync(email);
                if (result.Email is not null)
                    return new ResponseDTO<UserDTo>
                    {
                        IsSuccess = true,
                        Data = result,
                        Message = "User Found "
                    };
                return new ResponseDTO<UserDTo>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "User Not Found",
                    ErrorCode = ErrorCodes.NotFound
                };
            }
            catch (Exception)
            {
                return new ResponseDTO<UserDTo> { IsSuccess = false, Data = null, Message = "An Error occured", ErrorCode = ErrorCodes.Exception };
            }


        }

        public async Task<ResponseDTO<object>> CheckUserWithPassword(LoginDTO loginDTo)
        {
            try
            {
                var result = await _userRepository.CheckUserWithPasswordAsync(loginDTo);
                if (result.Succeeded)
                    return new ResponseDTO<object> { IsSuccess = true, Data = null, Message = "User verified" };
               return new ResponseDTO<object> {
                   IsSuccess = false,
                   Data = null,
                   Message = "User verification failed",
                   ErrorCode = ErrorCodes.BadRequest
               };
            }
            catch (Exception ex) {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = $"Exception happend {ex}",
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDTO<object>> DefaultRegister(RegisterDTO dto)
        {
            try
            {
                // Validate the DTO using FluentValidation
                var validationResult = await _registerValidator.ValidateAsync(dto);
                if(!validationResult.IsValid)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = string.Join(",", validationResult.Errors.Select(x=>x.ErrorMessage)),
                        ErrorCode = ErrorCodes.ValidationError
                    };
                }
                var result = await _userRepository.RegisterAsync(dto);
                if (result.Succeeded)
                {
                    //Add role for it as user
                    var roleResult = await _roleRepository.AddRoleToUserAsync(dto.Email, "user");
                    if (roleResult)
                    {
                        return new ResponseDTO<object>
                        {
                            IsSuccess = true,
                            Data = null,
                            Message = "User registered successfully and role assigned"
                        };
                    }
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = "User registered but role assignment failed",
                        ErrorCode = ErrorCodes.BadRequest
                    };
                }
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = string.Join(",", result.Errors.Select(x => x.Description)),
                    ErrorCode = ErrorCodes.BadRequest
                };

            }
            catch (Exception)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "An Error occured during registration",
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDTO<object>> Login(LoginDTO loginDTo)
        {
            try
            {
                var result =await  _userRepository.LoginAsync(loginDTo);
                if (result.Succeeded)
                {
                    var checkuser = await _userRepository.CheckUserAsync(loginDTo.Email);
                    var tokenDto = new TokenDTO
                    {
                        Email = loginDTo.Email,
                        Id = checkuser.id,
                        Role = checkuser.Role
                    };
                    var token = _tokenHelper.GenerateToken(tokenDto);
                    return new ResponseDTO<object> { IsSuccess = true, Data = token, Message = "Login successful" };
                }

                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Login failed",
                    ErrorCode = ErrorCodes.BadRequest
                };
            }
            catch (Exception)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "An Error occured during login",
                    ErrorCode = ErrorCodes.Exception
                };

            }
        }

        public async Task Logout()
        {
            await _userRepository.LogoutAsync();
        }

        public async Task<ResponseDTO<object>> Register(RegisterDTO registerDTO)
        {
            try
            {
                //Validate the DTO using FluentValidation
                var validationResult = await _registerValidator.ValidateAsync(registerDTO);
                if (!validationResult.IsValid)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)),
                        ErrorCode = ErrorCodes.ValidationError
                    };
                }
                var result = await _userRepository.RegisterAsync(registerDTO);
                if (result.Succeeded)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Data = null,
                        Message = "User registered successfully"
                    };
                }
           return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = string.Join(",", result.Errors.Select(x => x.Description)),
                    ErrorCode = ErrorCodes.BadRequest
                };
            }
            catch (Exception)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "An Error occured during registration",
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }
    }
}
