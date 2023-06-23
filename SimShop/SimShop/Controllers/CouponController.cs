using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimShop.Base.Constants;
using SimShop.Base;
using SimShop.Operation;
using SimShop.Schema;
using System.Data;
using System.Collections.Generic;
using SimShop.Data;
using System.Linq;
using System;

namespace SimShop.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : Controller
    {
       private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Customer}")]
        public ApiResponse<List<CouponResponse>> GetUserCoupon()
        {
            try
            {
                var userIdClaims = User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
                if (userIdClaims == null)
                {
                    throw new Exception(WarningType.LoginFail);
                }

                var userId = Convert.ToInt32(userIdClaims.Value);
                var response = _couponService.GetUserCoupons(userId);
                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CouponResponse>>(ex.Message);
            }
        }

        [HttpPost("AddCoupon")]
        [Authorize(Roles = $"{UserRoles.Admin}")]
        public ApiResponse CreateCoupon([FromBody] CouponRequest request)
        {
            var response = _couponService.CouponCreate(request);
            return response;
        }

    }
}
