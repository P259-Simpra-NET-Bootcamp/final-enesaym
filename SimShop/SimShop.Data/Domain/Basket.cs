using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimShop.Base;

namespace SimShop.Data.Domain
{
    public class Basket :BaseModel
    {
        public int UserId { get; set; } 
        public User User { get; set; } 
        public ICollection<BasketItem> BasketItems { get; set; } 

    }
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.ToTable("Baskets"); 
             
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();

            // User table ile ilişki
            builder.HasOne(x => x.User)
                .WithOne(x => x.Basket)
                .HasForeignKey<Basket>(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            //Basketitems table ile ilişki
            builder.HasMany(x => x.BasketItems)
                .WithOne(x => x.Basket)
                .HasForeignKey(x => x.BasketId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
