using AutoMapper;
using FluentValidation;
using Maqha.Core.Entities;
using Maqha.Core.IRepository;
using Maqha.Core.Services;
using Maqha.Utilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/cafe-info")]
    [ApiController]
    public class CafeInfoController : BaseController
    {
        //Unit of Work Pattern
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCafeInfoDTO> _createCafeInfoValidator;
        private readonly IMapper _mapper;
        private readonly IImageUploadService _imageUploadService;
      //  private readonly IValidator<UpdateCafeInfoDTO> _updateCafeInfoValidator;

        public CafeInfoController(IUnitOfWork unitOfWork, IValidator<CreateCafeInfoDTO> createCafeInfoValidator, IMapper mapper, IImageUploadService imageUploadService)
        {
            _unitOfWork = unitOfWork;
            _createCafeInfoValidator = createCafeInfoValidator;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
           // _updateCafeInfoValidator = updateCafeInfoValidator;
        }

        [HttpGet("GetAllCafeInfo")]
        public async Task<IActionResult> GetAllCafeInfo()
        {
            try
            {
                var cafeInfoRepo = _unitOfWork.Repository<CafeInfo>();
                var cafeInfos = await cafeInfoRepo.GetAllAsync();
                if (cafeInfos == null || !cafeInfos.Any())
                {
                    return CreateResponse(new ResponseDTO<IEnumerable<CafeInfo>>
                    {
                        IsSuccess = false,
                        Message = "No cafe information found."
                    });
                }
                return CreateResponse(new ResponseDTO<IEnumerable<CafeInfo>>
                {
                    IsSuccess = true,
                    Data = cafeInfos
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<IEnumerable<CafeInfo>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }

        }
        [HttpGet("GetCafeInfoById/{id}")]
        public async Task<IActionResult> GetCafeInfoById(int id)
        {
            try
            {
                var cafeInfoRepo = _unitOfWork.Repository<CafeInfo>();
                var cafeInfo = await cafeInfoRepo.GetByIdAsync(id);
                if (cafeInfo == null)
                {
                    return CreateResponse(new ResponseDTO<CafeInfo>
                    {
                        IsSuccess = false,
                        Message = "Cafe information not found."
                    });
                }
                return CreateResponse(new ResponseDTO<CafeInfo>
                {
                    IsSuccess = true,
                    Data = cafeInfo
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<CafeInfo>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPost("CreateCafeInfo")]
        public async Task<IActionResult> CreateCafeInfo([FromBody] CreateCafeInfoDTO createCafeInfoDTO)
        {
            try
            {
                var validationResult = await _createCafeInfoValidator.ValidateAsync(createCafeInfoDTO);
                if (!validationResult.IsValid)
                {
                    return CreateResponse(new ResponseDTO<CreateCafeInfoDTO>
                    {
                        IsSuccess = false,
                        Message = validationResult.Errors.Select(e => e.ErrorMessage).ToList().FirstOrDefault() ?? "Validation failed",
                        ErrorCode = ErrorCodes.ValidationError,
                    });
                }
                var cafeInfo = _mapper.Map<CafeInfo>(createCafeInfoDTO);
                var cafeInfoRepo = _unitOfWork.Repository<CafeInfo>();
                await cafeInfoRepo.AddAsync(cafeInfo);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<CafeInfo>
                {
                    IsSuccess = true,
                    Data = cafeInfo
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<CreateCafeInfoDTO>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("CreateCafeInfoWithUploadImage")]
        public async Task<IActionResult> CreatCafInfoWithImageUpload([FromForm] CreateCafeInfoRequest request)
        {
            try
            {
                //image pathe
                string imagePath = string.Empty;
                if (request.ImageUrl != null)
                {
                    //upload image to the server and get the image URL
                    imagePath = await _imageUploadService.UploadImageAsync(request.ImageUrl, "cafeinfo");
                }
                //map the request to the entity
                var cafeInfo = new CafeInfo
                {
                    Name = request.Name,
                    Description = request.Description,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    ImageUrl = imagePath, // Set the uploaded image URL
                    OpeningHours = request.OpeningHours,
                    WebsitUrl = request.WebsitUrl,
                    FacebookUrl = request.FacebookUrl,
                    InstagramUrl = request.InstagramUrl,
                    TwitterUrl = request.TwitterUrl,
                    YoutubeUrl = request.YoutubeUrl
                };
                await _unitOfWork.Repository<CafeInfo>().AddAsync(cafeInfo);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<CafeInfo>
                {
                    IsSuccess = true,
                    Data = cafeInfo
                });
            }
            catch (Exception)
            {
                return CreateResponse(new ResponseDTO<CreateCafeInfoRequest>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating cafe info with image upload."
                });
            }
        }
        [HttpDelete("DeleteCafeInfo/{id}")]
        public async Task<IActionResult> DeleteCafeInfo(int id)
        {
            try
            {
                var cafeInfoRepo = _unitOfWork.Repository<CafeInfo>();
                var cafeInfo = await cafeInfoRepo.GetByIdAsync(id);
                if (cafeInfo == null)
                {
                    return CreateResponse(new ResponseDTO<CafeInfo>
                    {
                        IsSuccess = false,
                        Message = "Cafe information not found."
                    });
                }
                await _imageUploadService.DeleteImageAsync(cafeInfo.ImageUrl);
                await cafeInfoRepo.DeleteAsync(cafeInfo);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<CafeInfo>
                {
                    IsSuccess = true,
                    Data = cafeInfo
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<CafeInfo>
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPut("UpdateCafeInfo{CafeId}")]
        public async Task<IActionResult> UpdateCafeInfo([FromForm]UpdateCafeInfoDTO dto,int CafeId )
        {
            try {
                  await _unitOfWork.Repository<CafeInfo>().UpdateAsync(CafeId, entity => 
                {
                    if (!string.IsNullOrEmpty(dto.Name)) entity.Name = dto.Name;
                    if (!string.IsNullOrEmpty(dto.Description)) entity.Description = dto.Description;
                    if (!string.IsNullOrEmpty(dto.Address)) entity.Address = dto.Address;
                    if (!string.IsNullOrEmpty(dto.PhoneNumber)) entity.PhoneNumber = dto.PhoneNumber;
                    if (!string.IsNullOrEmpty(dto.Email)) entity.Email = dto.Email;
                    if (!string.IsNullOrEmpty(dto.OpeningHours)) entity.OpeningHours = dto.OpeningHours;
                    if (!string.IsNullOrEmpty(dto.WebsitUrl)) entity.WebsitUrl = dto.WebsitUrl;
                    if (!string.IsNullOrEmpty(dto.FacebookUrl)) entity.FacebookUrl = dto.FacebookUrl;
                    if (!string.IsNullOrEmpty(dto.InstagramUrl)) entity.InstagramUrl = dto.InstagramUrl;
                    if (!string.IsNullOrEmpty(dto.TwitterUrl)) entity.TwitterUrl = dto.TwitterUrl;
                    if (!string.IsNullOrEmpty(dto.YoutubeUrl)) entity.YoutubeUrl = dto.YoutubeUrl;
                    if (dto.ImageFile != null)
                    {
                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(entity.ImageUrl))
                        {
                            _imageUploadService.DeleteImageAsync(entity.ImageUrl).Wait();
                        }
                        // Upload the new image and set the URL
                        entity.ImageUrl = _imageUploadService.UploadImageAsync(dto.ImageFile, "cafeinfo").Result;
                    }else if(!string.IsNullOrEmpty(dto.ImageUrl)) 
                    { 
                        entity.ImageUrl = dto.ImageUrl; // If no new image is uploaded, keep the existing one
                    }
                });
                await _unitOfWork.CompleteAsync();
                var updateEntity = await _unitOfWork.Repository<CafeInfo>().GetByIdAsync(CafeId);
                var responsDTo = _mapper.Map<UpdateCafeInfoDTO>(updateEntity);
                return CreateResponse(new ResponseDTO<UpdateCafeInfoDTO>
                {
                    IsSuccess = true,
                    Message = "Cafe information updated successfully.",
                    Data = responsDTo
                });
            }
            catch (Exception)
            {
                return CreateResponse(new ResponseDTO<UpdateCafeInfoDTO>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating cafe information."
                });

            }
        }
    }

}
