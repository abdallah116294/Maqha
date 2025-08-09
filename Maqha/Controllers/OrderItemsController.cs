using AutoMapper;
using Maqha.Core.Entities;
using Maqha.Core.IRepository;
using Maqha.Utilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/orderItems")]
    [ApiController]
    public class OrderItemsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapped;
        public OrderItemsController(IUnitOfWork unitOfWork, IMapper mapped)
        {
            _unitOfWork = unitOfWork;
            _mapped = mapped;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(CreateOrderItemDTO dto)
        {
            try
            {
                var mappedOrderItem = _mapped.Map<OrderItem>(dto);
                var orderItemRepository = _unitOfWork.Repository<OrderItem>();
                var menuItemRpository = _unitOfWork.Repository<MenuItem>();
                var menuItemExistsOrNot= await menuItemRpository.GetByIdAsync(dto.MenuItemId);
                if (menuItemExistsOrNot == null)
                {
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Menu Item Not Found",
                        ErrorCode = ErrorCodes.NotFound
                    });
                }
                await orderItemRepository.AddAsync(mappedOrderItem);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Order Item Created Successful",
                });

            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Accured Happened While Creating Order Item {ex}",
                    ErrorCode = ErrorCodes.Exception
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrderItems()
        {
            try
            {
                var orderItemRepository = _unitOfWork.Repository<OrderItem>();
                var OrderItems = await orderItemRepository.GetAllAsync();
                if (OrderItems == null)
                {
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "No Order Itmes Found",
                        ErrorCode = ErrorCodes.NotFound,
                    });
                }
                return Ok(CreateResponse(new ResponseDTO<List<OrderItem>>
                {
                    IsSuccess = true,
                    Message = "Get All Order Itmes Success",
                    Data = OrderItems,
                }));
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"Error Accured While Getting the Order Itmes {ex}",
                    ErrorCode = ErrorCodes.Exception,
                });

            }
        }

    }
}
