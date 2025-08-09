using Maqha.Core.IRepository;
using Maqha.Core.Services;
using Maqha.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ResponseDTO<object>> AddRoleToUserAsync(string email, string roleName)
        {
            try
            {
                var result = await _roleRepository.AddRoleToUserAsync(email, roleName);
                if (result)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Data =null,
                        Message = "Role added to user successfully."
                    };
                }
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "Failed to add role to user.",
                    Data = null,
                    ErrorCode = ErrorCodes.BadRequest
                };
            }
            catch (Exception)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding role to user.",
                    Data=null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDTO<object>> CreateRoleAsync(string roleName)
        {
            try
            {
                var result = await _roleRepository.CreateRoleAsync(roleName);
                if (result)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Data = null,
                        Message = "Role created successfully."
                    };
                } 
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "Failed to create role.",
                    Data=null,
                    ErrorCode = ErrorCodes.BadRequest
                };
            }
            catch (Exception)
            {
              return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the role.",
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDTO<List<string>>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllRolesAsync();
                if(roles is not null && roles.Any())
                {
                    return new ResponseDTO<List<string>>
                    {
                        IsSuccess = true,
                        Data = roles.ToList(),
                        Message = "Roles retrieved successfully."
                    };
                }
              return new ResponseDTO<List<string>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "No roles found.",
                    ErrorCode = ErrorCodes.NotFound
                };
            }
            catch (Exception)
            {
                return new ResponseDTO<List<string>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "An error occurred while retrieving roles.",
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDTO<object>> RemoveRoleAsync(string roleName)
        {
            try
            {
                var result = await _roleRepository.RemoveRoleAsync(roleName);
                if (result)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Message = "Role removed successfully."
                    };
                }
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "Failed to remove role.",
                    ErrorCode = ErrorCodes.BadRequest
                };
            }
            catch(Exception)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while removing the role.",
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }
        public async Task<ResponseDTO<object>>GetUserRoles(string userEmail)
        {
            try
            {
                    var roles = await _roleRepository.GetUserRoles(userEmail);
                    if (roles == null)
                    {
                        return new ResponseDTO<object> 
                        {
                            IsSuccess=false,
                            Message="No roles assigned to this user",
                            ErrorCode=ErrorCodes.NotFound,
                        };
                    } 
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Data = roles,
                    Message = "Get User Roles",

                };
            }
            catch (Exception)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    
                    Message = "an Error Accured to get Roles ",

                };
            }
        }
    }
}
