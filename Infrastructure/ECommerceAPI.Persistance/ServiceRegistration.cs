using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Services.Authentication;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities.Identity;
using ECommerceAPI.Persistance.Contexts;
using ECommerceAPI.Persistance.Repositories;
using ECommerceAPI.Persistance.Repositories.File;
using ECommerceAPI.Persistance.Repositories.InvoiceFile;
using ECommerceAPI.Persistance.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceAPI.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            
            services.AddDbContext<ECommerceAPIDbContext>(options => options.UseNpgsql(Configuration.ConnectingString));
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
            }).AddRoles<AppRole>().AddEntityFrameworkStores<ECommerceAPIDbContext>().AddDefaultTokenProviders();

            

            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();  
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();  
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>(); 
            services.AddScoped<IUserService,UserService>(); 
            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();  
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthService,AuthService>();
            services.AddScoped<IInternalAuthService, AuthService>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICompletedOrderReadRepository, CompletedOrderReadRepository>();
            services.AddScoped<ICompletedOrderWriteRepository, CompletedOrderWriteRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProductService, ProductService>();
            
            

        }
    }
}
