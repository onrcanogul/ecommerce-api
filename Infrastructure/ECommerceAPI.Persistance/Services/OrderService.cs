using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
   
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;
        readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
        readonly ICompletedOrderReadRepository _completedOrderReadRepository;
        readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository, ICompletedOrderReadRepository completedOrderReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
            _completedOrderReadRepository = completedOrderReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            await _orderWriteRepository.AddAsync(new()
            {
                Address = createOrder.Address,
                Description = createOrder.Description,
                Id = Guid.Parse(createOrder.BasketId),
                OrderCode = isUnique()
            }) ;
            await _orderWriteRepository.SaveAsync();
        }

        

        public async Task<SingleOrder> GetOrderByIdAsync(string id)
        {
            var query = _orderReadRepository.Table.Include(o => o.Basket)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(bi => bi.Product);

            var newOrder = await (from order in query
                           join completedOrder in _completedOrderReadRepository.Table
                           on order.Id equals completedOrder.OrderId into co
                           from _co in co.DefaultIfEmpty()
                           select new
                           {
                               Address = order.Address,
                               Id = order.Id,
                               CreatedDate = order.CreatedDate,
                               Description = order.Description,
                               Basket = order.Basket,
                               OrderCode = order.OrderCode,
                               Completed = _co != null ? true : false
                           }).FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));



            return new()
            {
                Address = newOrder.Address,
                Id = newOrder.Id.ToString(),
                BasketItems = newOrder.Basket.BasketItems.Select(bi => new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity
                }),
                CreatedDate = newOrder.CreatedDate,
                Description = newOrder.Description,
                OrderCode = newOrder.OrderCode,
                Completed = newOrder.Completed
            };

        }
        public async Task<(bool, CompletedOrderDto)> CompleteOrderAsync(string orderId)
        {
            Order? order = await _orderReadRepository.Table
                .Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(o => o.Id == Guid.Parse(orderId));
            if(order != null)
            {
               await  _completedOrderWriteRepository.AddAsync(new CompletedOrder { OrderId = Guid.Parse(orderId) });
               await _completedOrderWriteRepository.SaveAsync();
               return (true, new()
               {
                 OrderCode = order.OrderCode,
                 OrderDate = order.CreatedDate,
                 UserName = order.Basket.User.UserName,
                 To = order.Basket.User.Email
               });
            }
            return (false, null);
        }

        private string isUnique()
        {
            var orderCode = (new Random().Next() + new Random().Next());
            var orderCodeString = orderCode.ToString();           
            if (_orderWriteRepository.Table.Any(o => o.OrderCode == orderCodeString) | orderCode<1000)
                isUnique();


            return orderCodeString;           
        }

        public async Task DeleteOrder(string id)
        {
            var order = await _orderReadRepository.GetByIdAsync(id);
            if (order != null)          
            {
                _orderWriteRepository.Remove(order);
                await _orderWriteRepository.SaveAsync();
            }          
            else
                throw new Exception("Order is not found");
        }

        public async Task<GetAllOrders> GetAllOrdersAsync(int page, int size)
        {
            IQueryable<Order> query = getAllQueries().AsQueryable();

            return await getAllsResults(query, page, size);
        }

        public async Task<GetAllOrders> GetActiveUsersOrdersAsync(int page, int size)
        {
            string id = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var query = getAllQueries().Where(o => o.Basket.UserId == id);

            return await getAllsResults(query, page, size);

        }
        private IIncludableQueryable<Order, Product> getAllQueries()
        {
            return _orderReadRepository.Table
                .Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .Include(o => o.Basket)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(b => b.Product);               
        }

        private async Task<GetAllOrders> getAllsResults(IQueryable<Order> query, int page, int size)
        {
           var datas = query.Skip(page * size).Take(size);
            var newDatas = from order in datas
                           join completeOrder in _completedOrderReadRepository.Table
                           on order.Id equals completeOrder.OrderId into co
                           from _co in co.DefaultIfEmpty()
                           select new
                           {
                               Id = order.Id,
                               CreatedDate = order.CreatedDate,
                               Basket = order.Basket,
                               OrderCode = order.OrderCode,
                               Completed = _co != null ? true : false
                           };
            return new()
            {
                Order = await newDatas.Select(o => new
                {
                    Id = o.Id,
                    CreatedDate = o.CreatedDate,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                    Username = o.Basket.User.UserName,
                    Completed = o.Completed
                }).ToListAsync(),
                TotalCount = await query.CountAsync()
            };
        }
    }
}
