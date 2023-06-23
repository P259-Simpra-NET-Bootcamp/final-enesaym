using SimShop.Base;
using SimShop.Data;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public interface ICouponService
{
    ApiResponse CouponCreate(CouponRequest request);
    ApiResponse<List<CouponResponse>> GetUserCoupons(int userId);
}
