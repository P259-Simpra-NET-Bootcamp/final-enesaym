using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SimShop.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimShop.Base;

namespace SimShop.Data;

public class BasketItem:BaseModel
{
    public int Quantity { get; set; }
    public int BasketId { get; set; } 
    public Basket Basket { get; set; } 
    public int ProductId { get; set; } 
    public Product Product { get; set; }
}
public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.ToTable("BasketItems"); 
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();

        //basket ile ilişkiler
        builder.HasOne(x => x.Basket)
            .WithMany(x => x.BasketItems)
            .HasForeignKey(x => x.BasketId)
            .IsRequired();

        // product ile ilişkiler
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .IsRequired();
    }
}
