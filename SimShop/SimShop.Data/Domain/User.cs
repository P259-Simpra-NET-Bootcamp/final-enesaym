using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimShop.Base;
using SimShop.Data.Domain;
using System.Reflection.Emit;

namespace SimShop.Data;

public class User : BaseModel
{
    
    public string Name { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public decimal Wallet { get; set; }
    public bool Status { get; set; }
    public String Role { get; set; }
    public Basket Basket { get; set; }
    public List<Order> Orders { get; set; }
    public List<Coupon> Coupons { get; set; }

}
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        
        builder.Property(x => x.Id).IsRequired().UseIdentityColumn();
        builder.Property(x => x.Wallet).IsRequired(true).HasPrecision(15, 2).HasDefaultValue(0);
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(20);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(15);
        builder.Property(x => x.UserName).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Password).IsRequired(true);
        builder.Property(x => x.Email).IsRequired(false).HasMaxLength(20);

        builder.HasMany(u => u.Orders) // Kullanıcının birden çok siparişi olur
       .WithOne(o => o.User) // Siparişin kullanıcısı 
       .HasForeignKey(o => o.UserId); 

        builder.HasMany(u => u.Coupons) // Kullanıcının birden çok kuponu olur
       .WithOne(o => o.User) // Siparişin kullanıcısı 
       .HasForeignKey(o => o.UserId);


        builder.HasIndex(x => x.UserName).IsUnique(true);
        builder.HasOne(x => x.Basket) // Kullanıcının bir sepeti olacak
               .WithOne(x => x.User) // Sepetin kullanıcısı
               .HasForeignKey<Basket>(x => x.UserId); 


        // sistem kullanıcısının eklenmesi
        builder.HasData(new User
        {
            Id = 1,
            Name = "Admin",
            LastName = "Admin",
            UserName = "admin",
            Password = "21232f297a57a5a743894a0e4a801fc3",
            Email = "admin@example.com",
            Wallet = 0,
            Status = true,
            Role = "admin",
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        });


    }


}
