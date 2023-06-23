using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SimShop.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SimShop.Data;

public class Category:BaseModel
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string Tag { get; set; }
    public virtual List<Product> Products { get; set; }

}
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Id).IsRequired(true).UseIdentityColumn();
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Url).IsRequired(false).HasMaxLength(20);
        builder.Property(x => x.Tag).IsRequired(false).HasMaxLength(30);

        builder.HasIndex(x => x.Name).IsUnique(true);
        builder.HasMany(x => x.Products)
               .WithOne(x => x.Category)
               .IsRequired(false);

    }
}