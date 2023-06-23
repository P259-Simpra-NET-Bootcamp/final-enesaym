using Microsoft.Extensions.DependencyInjection;
using SimShop.Operation;

namespace SimShop.Service.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceExtension(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICouponService, CouponService>();
        }
    }
}
