using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Common;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ECommerceAPI.Persistance.Contexts
{
    public class ECommerceAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public ECommerceAPIDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Domain.Entities.File> Files{ get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<CompletedOrder> CompletedOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasKey(o => o.Id);

            //builder.Entity<Product>()
            //     .HasOne(p => p.User)
            //     .WithMany(u => u.Products)
            //     .HasForeignKey(p => p.UserId);


            builder.Entity<Order>()
                .HasIndex(o => o.OrderCode)
                .IsUnique();

            builder.Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Basket)
                .HasForeignKey<Order>(b => b.Id);

            builder.Entity<Order>()
                .HasOne(o => o.CompletedOrder)
                .WithOne(c => c.Order)
                .HasForeignKey<CompletedOrder>(c => c.OrderId);

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            { 
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };

            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
