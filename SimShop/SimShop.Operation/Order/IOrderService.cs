using SimShop.Base;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public interface IOrderService
{
    ApiResponse CreateOrder(int userId,int CouponCode);
    ApiResponse<List<OrderResponse>> GetAllUserOrders(int userId);
}
