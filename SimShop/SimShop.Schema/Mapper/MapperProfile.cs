using AutoMapper;
using SimShop.Data;
using SimShop.Data.Domain;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Schema;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Category, CategoryResponse>();
        CreateMap<CategoryRequest, Category>();

        CreateMap<Product, ProductResponse>();
        CreateMap<ProductRequest, Product>();

        CreateMap<UserRequest, User>();

        CreateMap<Basket, BasketResponse>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.BasketItems
            .Sum(bi => bi.Product.Price * bi.Quantity)));

        CreateMap<BasketItem, BasketItemResponse>();

        CreateMap<CouponRequest, Coupon>();
        CreateMap<Coupon,CouponResponse >();

        CreateMap<Order, OrderResponse>();
        CreateMap<OrderItem, OrderItemResponse>();

        CreateMap<User,UserResponse >();
    }
}
