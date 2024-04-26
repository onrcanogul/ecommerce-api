using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.ViewModels.Baskets;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;
using ECommerceAPI.Persistance.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IBasketWriteRepository _basketWriteRepository;
        private readonly IBasketItemWriteRepository _basketItemWriteRepository;
        private readonly IBasketItemReadRepository _basketItemReadRepository;
        private readonly IBasketReadRepository _basketReadRepository;

        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            var username =_httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
               AppUser? user= await _userManager.Users
                            .Include(u => u.Baskets)
                            .FirstOrDefaultAsync(u => u.UserName == username);

                var _basket = from basket in user.Baskets
                              join order in _orderReadRepository.Table
                              on basket.Id equals order.Id into orders
                              from order in orders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order,
                              };
                Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null))
                {
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null).Basket;
                }
                else
                {
                    targetBasket = new();
                    user.Baskets.Add(targetBasket);
                }
                await _basketWriteRepository.SaveAsync();
                return targetBasket;
                                    
            }
            throw new Exception("Unexpected Error");


        }

        public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();
            if (basket != null)
            {
               BasketItem _basketItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));
                if(_basketItem != null)
                {
                    _basketItem.Quantity++;
                }
                else
                {
                    await _basketItemWriteRepository.AddAsync(new()
                    {
                        BasketId = basket.Id,
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity,
                    }
                    );
                }
                await _basketItemWriteRepository.SaveAsync();
            }
            
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket = await ContextUser();
            Basket? result = await _basketReadRepository.Table.Include(b => b.BasketItems).ThenInclude(bi => bi.Product).FirstOrDefaultAsync(b => b.Id == basket.Id);

            return result.BasketItems.ToList();

            //if(basket != null)
            //{
            //    _basketItemReadRepository.Table.Include(b=>b.Basket).Include(bi => bi.Product).Include(bi => bi.Basket).Where(bi => bi.BasketId == basket.Id).ToList();
            //}

        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if(basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }

        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem _basketItem = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
            if(_basketItem != null)
            {
                _basketItem.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public Basket? GetUserActiveBasket
        {
            get
            {
                return ContextUser().Result;
            }
        }
    }
}
