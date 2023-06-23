using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimShop.Data.Context;

namespace SimShop.Service.Extensions;

public static class DbContextExtension
{
    public static void AddDbContextExtension(this IServiceCollection services, IConfiguration Configuration)
    {
        var dbType = Configuration.GetConnectionString("DbType");
        var dbConfig = Configuration.GetConnectionString("MsSqlConnection");
        services.AddDbContext<SimShopDbContext>(opts =>
        opts.UseSqlServer(dbConfig));
    }
}