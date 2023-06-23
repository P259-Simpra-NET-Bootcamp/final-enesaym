using Microsoft.EntityFrameworkCore;
using SimShop.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Data.Context
{
    public class SimShopDbContext:DbContext
    {
        public SimShopDbContext(DbContextOptions<SimShopDbContext> options ):base(options)
        {
            
        }
        public DbSet<Category>  Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Basket> Basket { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<BasketItem> BasketItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<Order> OrderItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new BasketConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BasketItemConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new CouponConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
