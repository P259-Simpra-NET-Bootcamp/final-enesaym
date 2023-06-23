using SimShop.Base;
using SimShop.Data;
using SimShop.Data.Domain;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public interface IBasketService
{
    ApiResponse Insert(int userId,int productİd);
    ApiResponse<BasketResponse> GetBasket(int userId);
    ApiResponse DeleteBasketItem(int userId, int productId);
}
