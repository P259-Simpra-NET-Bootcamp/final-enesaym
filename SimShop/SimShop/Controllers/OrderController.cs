using Microsoft.AspNetCore.Mvc;
using SimShop.Base;
using SimShop.Data.Context;
using SimShop.Operation;
using System.Linq;
using System;
using SimShop.Base.Constants;
using System.Collections.Generic;
using SimShop.Schema;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SimShop.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Customer}")]
        public ApiResponse<List<OrderResponse>> GetOrder()
        {
            try
            {
                var userIdClaims = User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
                if (userIdClaims == null)
                {
                    throw new Exception(WarningType.LoginFail);
                }

                var userId = Convert.ToInt32(userIdClaims.Value);
                var response = _orderService.GetAllUserOrders(userId);
                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderResponse>>(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Customer}")]
        public ApiResponse CreateOrder(int couponCode,int CardNumber)
        {
            try
            {
                var userIdClaims = User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
                if (userIdClaims == null)
                {
                    throw new Exception(WarningType.LoginFail);
                }

                var userId = Convert.ToInt32(userIdClaims.Value);
                var response = _orderService.CreateOrder(userId, couponCode);
                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

    }
}
