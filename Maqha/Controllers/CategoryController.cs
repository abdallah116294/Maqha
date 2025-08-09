using AutoMapper;
using Maqha.Core.Entities;
using Maqha.Core.IRepository;
using Maqha.Utilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryDTO dto)
        {
            if (dto == null)
            {
                return CreateResponse(new ResponseDTO<CreateCategoryDTO>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Category Data Cannot Be Null",
                    ErrorCode = ErrorCodes.BadRequest
                });
            }
            try
            {
                var catgeoryRepository = _unitOfWork.Repository<Category>();
                if(catgeoryRepository == null)
                {
                    return CreateResponse(new ResponseDTO<CreateCategoryDTO>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = "Category Repository Not Found",
                        ErrorCode = ErrorCodes.NotFound
                    });
                }
                var mappedCategory= _mapper.Map<Category>(dto);
                if (mappedCategory != null) 
                    await catgeoryRepository.AddAsync(mappedCategory);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<CreateCategoryDTO>
                {
                    IsSuccess = true,
                    Data = dto,
                    Message="Category Created Succesfull"
                });  
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<CreateCategoryDTO> 
                {
                    IsSuccess=false,
                    Data=null,
                    Message=$"An Error Accured While Creating Category{ex}",
                    ErrorCode=ErrorCodes.Exception
                });

            }
        }
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categorRepositor = _unitOfWork.Repository<Category>();
                var categories = await categorRepositor.GetAllAsync();
                if (categories.Count == 0)
                {
                    return CreateResponse(new ResponseDTO<List<ResultCategoryDTO>>
                    {
                        IsSuccess = false,
                        Message = "Not Categories Found",
                        Data = null,
                        ErrorCode=ErrorCodes.NotFound
                    });
                }
                var result = _mapper.Map<List<ResultCategoryDTO>>(categories);
                return CreateResponse(new ResponseDTO<List<ResultCategoryDTO>> 
                {
                    IsSuccess=true,
                    Message="Get Categories Successful",
                    Data=result,
                });

            }catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<List<ResultCategoryDTO>> 
                {
                    IsSuccess=false,
                    Data=null,
                    Message=$"An Error Accured While Getting Categories {ex}",
                    ErrorCode=ErrorCodes.Exception,
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult>GetAllCategoriesWithMenuItems()
        {
            try
            {
                var categoryRepository = _unitOfWork.Repository<Category>();
                var categories = await categoryRepository.GetAllAsync(includes: x=>x.MenuItems);
                if (categories.Count == 0)
                {
                    return CreateResponse(new ResponseDTO<List<ResultCategoryDTO>>
                    {
                        IsSuccess = false,
                        Message = "No Categories Found",
                        Data = null,
                        ErrorCode=ErrorCodes.NotFound
                    });
                }
                var result = _mapper.Map<List<ResultCategoryWithMenuItemDTO>>(categories);
                return CreateResponse(new ResponseDTO<List<ResultCategoryWithMenuItemDTO>> 
                {
                    IsSuccess=true,
                    Message="Get Categories Successful",
                    Data=result,
                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<List<ResultCategoryWithMenuItemDTO>> 
                {
                    IsSuccess=false,
                    Data=null,
                    Message=$"An Error Accured While Getting Categories {ex}",
                    ErrorCode=ErrorCodes.Exception
                });
            }
        }
        [HttpGet("GetCategoryById{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if (id == null)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "Id is null",
                    Data = null,
                    ErrorCode = ErrorCodes.BadRequest
                });
            }
            try
            {
                var categoryRepository = _unitOfWork.Repository<Category>();
                var category = await categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "No Category Found with this Id",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    });
                }
                var result = _mapper.Map<ResultCategoryDTO>(category);
                return CreateResponse(new ResponseDTO<ResultCategoryDTO>
                {
                    IsSuccess = true,
                    Message = "Get Category Successful",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Accured while Getting Category {ex}",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }
        [HttpPut("UpdateCategory{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm]UpdateCategoryDTO dto)
        {
            if (id == null || dto == null)
            {
                return CreateResponse(new ResponseDTO<UpdateCategoryDTO>
                {
                    IsSuccess = false,
                    Message = "Id or Category Data is null",
                    Data = null,
                    ErrorCode = ErrorCodes.BadRequest
                });
            }
            try
            {
                var categoryRepository = _unitOfWork.Repository<Category>();
                var category = await categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return CreateResponse(new ResponseDTO<UpdateCategoryDTO>
                    {
                        IsSuccess = false,
                        Message = "No Category Found with this Id",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    });
                }
                _mapper.Map(dto, category);
                await categoryRepository.UpdateAsync(category);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<UpdateCategoryDTO>
                {
                    IsSuccess = true,
                    Message = "Category Updated Successful",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<UpdateCategoryDTO>
                {
                    IsSuccess = false,
                    Message = $"An Error Accured while Updating Category {ex}",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }
        [HttpDelete("DeletCategory{id}")]
        public async Task<IActionResult>DeleteCategory(int id)
        {
            if (id == null)
            {
                return CreateResponse(new ResponseDTO<object> 
                {
                    IsSuccess=false,
                    Message="Id is null",
                    Data=null,
                    ErrorCode= ErrorCodes.BadRequest
                });
            }
            try
            {
                // var cateoryRepository = _unitOfWork.Repository<Category>();
                var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
                if (category == null)
                    return CreateResponse(new ResponseDTO<object> 
                    {
                        IsSuccess=false,
                        Message="No Category Found with this Id",
                        Data=null,
                        ErrorCode=ErrorCodes.NotFound
                    });
                await _unitOfWork.Repository<Category>().DeleteAsync(category);
                await _unitOfWork.CompleteAsync();
                return CreateResponse( new ResponseDTO<object>
                {
                    IsSuccess=true,
                    Message="Category Deleted Successful",
                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess=false,
                    Message=$"An Error Accured while Deleting Category {ex}",
                    ErrorCode=ErrorCodes.Exception
                });
            }
        }
    }
}
