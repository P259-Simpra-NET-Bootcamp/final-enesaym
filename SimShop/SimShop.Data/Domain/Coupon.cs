using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SimShop.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Data;

public class Coupon:BaseModel
{
    public int Code { get; set; }
    public decimal DiscountAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.Property(x => x.Code).IsRequired().HasMaxLength(10);
        builder.Property(x => x.DiscountAmount).IsRequired().HasColumnType("decimal(15, 2)").HasDefaultValue(0);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(x => x.ValidFrom).IsRequired();
        builder.Property(x => x.ValidTo).IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();
    }
}
