using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrder createOrder);
        Task<GetOrders> GetAllOrdersAsync(int page, int size);
        Task<SingleOrder> GetOrderByIdAsync(string id);
        Task<(bool,CompletedOrderDto)> CompleteOrderAsync(string orderId);
        Task DeleteOrder(string id);
        
        Task <GetOrders> GetActiveUsersOrdersAsync(int page, int size);


    }
}
