using Maqha.Core.Entities;
using Maqha.Core.IRepository;
using Maqha.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddOrderItemAsync(int orderId, OrderItem item)
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            var order = await orderRepository.GetByIdAsync(orderId,o=>o.OrderItems);
            if (order == null) return false;
            order.OrderItems.Add(item);
            order.TotalPrice=order.OrderItems.Sum(i=>i.Price*i.Quantity);
            await orderRepository.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            order.TotalPrice = order.OrderItems.Sum(item=>item.Price*item.Quantity);
            order.UpdatAt = DateTime.Now;
            await orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            var order=await orderRepository.GetByIdAsync(id);
            if (order == null) return false;
            await orderRepository.DeleteAsync(order);
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            return await orderRepository.GetAllAsync(includes: o => o.OrderItems);
        }

        public async Task<Order> GetOrderById(int id)
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            var order = await orderRepository.GetByIdAsync(id,includes:o=>o.OrderItems);
            return order;

        }

        public async Task<bool> RemoveOrderItemAsync(int orderId, int itemId)
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            var order = await orderRepository.GetByIdAsync(orderId, o => o.OrderItems);
            if (order == null) return false;
            var item=order.OrderItems.FirstOrDefault(i=>i.Id== itemId);
            if (item == null) return false;
            order.OrderItems.Remove(item);
            order.TotalPrice=order.OrderItems.Sum(i=>i.Price*i.Quantity);
            await orderRepository.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            order.TotalPrice = order.OrderItems.Sum(item => item.Price * item.Quantity);
            order.UpdatAt = DateTime.UtcNow;
             await orderRepository.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            var order = await orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;
            order.Status = status;
            order.UpdatAt=DateTime.UtcNow;
            await orderRepository.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
