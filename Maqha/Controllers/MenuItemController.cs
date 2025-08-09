using AutoMapper;
using Maqha.Core.Entities;
using Maqha.Core.IRepository;
using Maqha.Core.Services;
using Maqha.Utilities.DTO;
using Maqha.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/menuItem")]
    [ApiController]
    public class MenuItemController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageUploadService _imageUploadService;

        public MenuItemController(IUnitOfWork unitOfWork, IMapper mapper, IImageUploadService imageUploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
        }
        // Get All Menut Items 
        [Authorize(Roles ="admin")]
        //[AdminOnly]
        [HttpGet("GetAllMenuItem")]
        public async Task<IActionResult> GetAllMenuItems()
        {
            try
            {
                var menuItem=await _unitOfWork.Repository<MenuItem>().GetAllAsync
                    (includes:x=>x.Category);
                var menuItmeMapped = _mapper.Map<IEnumerable<ResultMenuItemDTO>>(menuItem);
                if (menuItem == null || !menuItem.Any())
                {
                    return CreateResponse(new ResponseDTO<IEnumerable<ResultMenuItemDTO>>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = "No Menu Items Found",
                        ErrorCode = ErrorCodes.NotFound
                    });
                }
                return CreateResponse(new ResponseDTO<IEnumerable<ResultMenuItemDTO>>
                {
                    IsSuccess = true,
                    Data = menuItmeMapped,
                    Message = "Menu Items Retrieved Successfully"
                });

            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<IEnumerable<ResultMenuItemDTO>> 
                {
                    IsSuccess= false,
                    Data = null,
                    Message = $"An error occurred while retrieving menu items: {ex.Message}",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }
        [HttpGet("GetMenuItmeByID{id}")]
        public async Task<IActionResult>GetMenuItemById(int id)
        {
            try
            {
                var menuItem = await _unitOfWork.Repository<MenuItem>().GetByIdAsync(id, includes: x => x.Category);
                var mappedMenuItem = _mapper.Map<ResultMenuItemDTO>(menuItem);
                if (menuItem == null)
                {
                    return CreateResponse(new ResponseDTO<ResultMenuItemDTO> 
                    {
                        IsSuccess=false,
                        Message="No Menu With this ID",
                        Data=null,
                        ErrorCode=ErrorCodes.NotFound
                    });
              
                }
                return CreateResponse(new ResponseDTO<ResultMenuItemDTO>
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = mappedMenuItem
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<ResultMenuItemDTO> 
                {
                    IsSuccess=false,
                    Message=$"An Error Accured While Getting Data {ex}",
                    Data=null,
                    ErrorCode=ErrorCodes.Exception
                });
            }
        }
        //[Authorize]
        //[AdminOnly]
        [HttpPost]
        public async Task<IActionResult>CreateMenuIte([FromForm] CreateMenuItmDTO dto)
        {
            try
            {
                var menuItemRepository = _unitOfWork.Repository<MenuItem>();
                //if (dto.Image == null&&dto.ImageFile!=null)
                //{
                        var imageUrl = await _imageUploadService.UploadImageAsync(dto.ImageFile!,"menuItme");
                        dto.Image = imageUrl;
                        var MappedMenuItem = _mapper.Map<MenuItem>(dto);
                       await menuItemRepository.AddAsync(MappedMenuItem);
                        await _unitOfWork.CompleteAsync();
                    return CreateResponse(new ResponseDTO<MenuItem>
                    {
                        IsSuccess = true,
                        Data = MappedMenuItem,
                        Message = "Menu Item Created Successfully"
                    });
               // }
               

            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<CreateMenuItmDTO>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = $"Error Accured while Creating Menu Item {ex}"
                });

            }
        }
        //[Authorize]
        //[AdminOnly] 
        [HttpDelete("DeleteMenuItem/{id}")]
        public async Task<IActionResult>DeleteMenuItem(int id)
        {
            if (id == null)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess =false,
                    Message="Id must not null",
                    ErrorCode=ErrorCodes.BadRequest,
                });
            }
            try
            {
                var menuItemRepo = _unitOfWork.Repository<MenuItem>();
                var menuItem = await menuItemRepo.GetByIdAsync(id);
                if (menuItem == null)
                {
                    return CreateResponse(new ResponseDTO<object> 
                    { 
                        IsSuccess =false,
                        Message="No MenuItem Found with this Id",
                        ErrorCode=ErrorCodes.NotFound,
                    });
                }
                await _imageUploadService.DeleteImageAsync(menuItem.Image);
                await menuItemRepo.DeleteAsync(menuItem);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<object> 
                {
                    IsSuccess=true,
                    Message="Successful Delete MenuItem",
                   
                });
            }catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"Error Accured While Deleting MenuItem {ex}",
                    ErrorCode=ErrorCodes.Exception
                });
            }
        }
        //[Authorize]
        //[AdminOnly]
        [HttpPut("UpdateMenuItmem{id}")]
        public async Task<IActionResult> UpdateMenuItem([FromForm]UpdateMenuItemDTO dto,int id) 
        {
            try
            {
                await _unitOfWork.Repository<MenuItem>().UpdateAsync(id, entity => 
                {
                    if (!string.IsNullOrEmpty(dto.Name))
                        entity.Name = dto.Name;

                    if (!string.IsNullOrEmpty(dto.Description))
                        entity.Description = dto.Description;

                    if (dto.Price.HasValue && dto.Price.Value > 0)
                        entity.Price = dto.Price.Value;

                    if (dto.IsAvailable.HasValue)
                        entity.IsAvailable = dto.IsAvailable.Value;

                    if (dto.CategoryId.HasValue && dto.CategoryId > 0)
                        entity.CategoryId = dto.CategoryId.Value;

                    if (dto.ImageFile != null)
                    {
                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(entity.Image))
                        {
                            _imageUploadService.DeleteImageAsync(entity.Image).Wait();
                        }
                        // Upload the new image and set the URL
                        entity.Image = _imageUploadService.UploadImageAsync(dto.ImageFile, "menuItem").Result;
                    }
                    else if (!string.IsNullOrEmpty(dto.Image))
                    {
                        entity.Image = dto.Image; // If no new image is uploaded, keep the existing one
                    }
                });
                await _unitOfWork.CompleteAsync();
                var menuItemAfterUpdate = await _unitOfWork.Repository<MenuItem>().GetByIdAsync(id, includes: x => x.Category);
                var mappedMenuItem = _mapper.Map<ResultMenuItemDTO>(menuItemAfterUpdate);
                return CreateResponse(new ResponseDTO<ResultMenuItemDTO>
                {
                    IsSuccess = true,
                    Message = "Update MenuItem Succesful",
                    Data=mappedMenuItem,
                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<ResultMenuItemDTO>
                {
                    IsSuccess = false,
                    Message = $"An Error Accured While Update MenuItem {ex}",
                    Data = null,
                    ErrorCode=ErrorCodes.Exception,
                });
            }
        }
     }

}
