using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SimShop.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Data;

public class Order:BaseModel
{
    public int OrderNumber { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal UsedPoints { get; set; }
    public decimal UsedCoupon { get; set; }
    public decimal EarnedPoints { get; set; }
    public List<OrderItem> OrderItems { get; set; }

}
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.OrderNumber).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.OrderDate).IsRequired();
        builder.Property(x => x.TotalAmount).IsRequired().HasPrecision(15, 2).HasDefaultValue(0); 
        builder.Property(x => x.PaidAmount).IsRequired().HasPrecision(15, 2).HasDefaultValue(0);
        builder.Property(x => x.UsedPoints).IsRequired().HasPrecision(15, 2).HasDefaultValue(0);
        builder.Property(x => x.UsedCoupon).IsRequired().HasPrecision(15, 2).HasDefaultValue(0);
        builder.Property(x => x.EarnedPoints).IsRequired().HasPrecision(15, 2).HasDefaultValue(0);

        builder.HasOne(x => x.User)
               .WithMany(x => x.Orders)
               .HasForeignKey(x => x.UserId);

       builder.HasMany(x => x.OrderItems)
       .WithOne(x => x.Order)
       .HasForeignKey(x => x.OrderId)
       .IsRequired();

    }
}
