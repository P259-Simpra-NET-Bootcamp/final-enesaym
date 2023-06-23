using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimShop.Base;
using SimShop.Data.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimShop.Data;

public class Product : BaseModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
    public int MaxPoint { get; set; }
    public int PointPercentage { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public Category Category { get; set; }

}
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Id).IsRequired(true).UseIdentityColumn();
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Stock).IsRequired(true).HasPrecision(15, 2).HasDefaultValue(0);
        builder.Property(x => x.IsAvailable).IsRequired(true).HasMaxLength(10);
        builder.Property(x => x.MaxPoint).IsRequired(true).HasMaxLength(10);
        builder.Property(x => x.PointPercentage).IsRequired(true).HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);
        builder.HasIndex(x => x.Name).IsUnique(true);

       
    }
}
