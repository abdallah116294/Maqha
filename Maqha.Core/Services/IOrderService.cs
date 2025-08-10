using Maqha.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.Services
{
    public interface IOrderService
    {
        //Create Order 
        Task<Order> CreateOrder(Order order);
        // Get Order By Id 
        Task<Order> GetOrderById(int id);

        //Get All Orders
        Task<IEnumerable<Order>> GetAllOrders();
        //Update Order 
        Task<bool> UpdateOrderAsync(Order order);
        //Delete Order
        Task<bool> DeleteOrderAsync(int id);
        //Add order item
       // Task<bool> AddOrderItemAsync(int orderId, OrderItem item);
        //remove order item
      //  Task<bool> RemoveOrderItemAsync(int orderId, int itemId);
        //update order status 
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
