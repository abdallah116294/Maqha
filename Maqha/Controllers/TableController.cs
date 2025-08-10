using AutoMapper;
using Maqha.Core.Entities;
using Maqha.Core.IRepository;
using Maqha.Utilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/table")]
    [ApiController]
    public class TableController : BaseController
    {
        //UnitOfwork to use GenericRepository
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TableController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // Get All Tables 
        [HttpGet("GetAllTables")]
        public async Task<IActionResult> GetAllTables ()
        {
            try
            {
                var tableCategory = _unitOfWork.Repository<Table>();
                var tables = await tableCategory.GetAllAsync();
                if (tables == null)
                    return CreateResponse(new ResponseDTO<object> 
                    {
                        IsSuccess=false,
                        Message="Not Tables Found",
                        ErrorCode=ErrorCodes.NotFound,
                    });
                return CreateResponse(new ResponseDTO<IEnumerable<Table>>
                {
                    IsSuccess = true,
                    Message = "Get Tables Succes",
                    Data = tables,
                });

            }catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"Error While Getting Tables {ex} ",
                    ErrorCode = ErrorCodes.Exception,
                });
            }
        }
        [HttpPost("CreateTable")]
        public async Task<IActionResult> CreateTable([FromForm]CreateTableDTO dto)
        {
            try
            {
                var tableRepository=_unitOfWork.Repository<Table>();
                var mappedTable = _mapper.Map<Table>(dto);
                 await tableRepository.AddAsync(mappedTable);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess=true,
                    Message="Add Table Succesful",
                    Data=mappedTable,
                });
            }catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess =false,
                    Message = $"Error While Add Table{ ex} ",
                   ErrorCode = ErrorCodes.Exception,
                });
            }
        }
        [HttpDelete("DeleteTable{id}")]
        public async Task<IActionResult>DeleteTable(int id) 
        {
            try
            {
                var tableRepository = _unitOfWork.Repository<Table>();
                var table = await tableRepository.GetByIdAsync(id);
                if (table == null)
                    return CreateResponse(new ResponseDTO<object> 
                    {
                        IsSuccess=false,
                        Message="No Table Found with this ID to Delete ",
                        ErrorCode=ErrorCodes.NotFound 
                    });
                await tableRepository.DeleteAsync(table);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<object> 
                {
                    IsSuccess=true,
                    Message="Delete Table Succesful",
                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"Error While Deleting  Table {ex}",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }
        [HttpGet("GetAllActivesTable")]
        public async Task<IActionResult> GetAllActiveTables() 
        {
            try
            {
                var tableRepository = _unitOfWork.Repository<Table>();
                var ActiveTables = await tableRepository.GetAllAsync(predicate:table=>table.IsActive==true);
                if (ActiveTables.Count()==0)
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess=true,
                        Message="No Table is Active",
                    });
                return CreateResponse(new ResponseDTO<IEnumerable<Table>> 
                {
                    IsSuccess=true,
                    Message="Get All Active Tables",
                    Data=ActiveTables,
                });

            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message =  $"Error While Getting Active Tables {ex}",
                    ErrorCode=ErrorCodes.Exception
                });
            }
        }
        [HttpGet("GetTableById{id}")]
        public async Task<IActionResult>GetTableById(int id)
        {
            try
            {
                var tableRepository = _unitOfWork.Repository<Table>();
                var table = await tableRepository.GetByIdAsync(id);
                if (table == null)
                    return CreateResponse(new ResponseDTO<object> 
                    {
                        IsSuccess=false,
                        Message="No Table Found",
                        ErrorCode=ErrorCodes.NotFound
                    });
                return CreateResponse(new ResponseDTO<Table>
                {
                    IsSuccess = true,
                    Message = "Get Table Succesful",
                   Data=table,
                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<Table>
                {
                    IsSuccess = false,
                    Message = $"Error While Getting Table {ex}",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }
        [HttpGet("GetTableByNumber{number}")]
        public async Task<IActionResult>GetTableByNumber(int number)
        {
            try
            {
                var tableRepository = _unitOfWork.Repository<Table>();
                var table = await tableRepository.GetAllAsync(predicate:table=>table.TableNumber==number);
                if (table == null)
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "No Table Found",
                        ErrorCode = ErrorCodes.NotFound
                    });
                return CreateResponse(new ResponseDTO<IEnumerable<Table>>
                {
                    IsSuccess = true,
                    Message = "Get Table Succesful",
                    Data = table,
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<Table>
                {
                    IsSuccess = false,
                    Message = $"Error While Getting Table {ex}",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }
        [HttpPut("UpdateTableStatusById{id}")]
        public async Task<IActionResult>UpdateTabelStatusByID(int id)
        {
            try
            {
                var tableRepository = _unitOfWork.Repository<Table>();
                var table = await tableRepository.GetByIdAsync(id);
                if (table == null)
                    return CreateResponse(new ResponseDTO<object> 
                    {
                        IsSuccess=false,
                        Message="Not Table Found",
                        ErrorCode=ErrorCodes.NotFound
                    });
                table.IsActive = !table.IsActive;
                await tableRepository.UpdateAsync(table);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<object> 
                {
                    IsSuccess=true,
                    Message="Update Status Successful",
                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"Error Accured While Update {ex} ",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }
        [HttpPost("UpdateTableStatusByNumber{number}")]
        public async Task<IActionResult> UpdateTableStatusByNumber(int number)
        {
            try
            {
                var tableRepository = _unitOfWork.Repository<Table>();
                var table = await tableRepository.GetAllAsync(predicate: table => table.TableNumber == number);
                if (table == null)
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Not Table Found",
                        ErrorCode = ErrorCodes.NotFound
                    });
                table.First().IsActive= !table.First().IsActive;
                await tableRepository.UpdateAsync(table.First());
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Update Status Successful",
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"Error Accured While Update {ex} ",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }

    }
}
