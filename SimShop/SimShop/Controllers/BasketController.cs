using Microsoft.AspNetCore.Mvc;
using SimShop.Data.Domain;
using System.Threading.Tasks;
using System;
using SimShop.Data.Context;
using SimShop.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SimShop.Base;
using SimShop.Operation;
using System.Collections.Generic;
using SimShop.Schema;
using SimShop.Base.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SimShop.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
       
        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
          
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Customer}")]
        public ApiResponse<BasketResponse> GetBasketItems()
        {
            try
            {
                var userIdClaims = User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
                if (userIdClaims == null)
                {
                    throw new Exception(WarningType.LoginFail);
                }
                var userId = Convert.ToInt32(userIdClaims.Value);
                var response = _basketService.GetBasket(userId);
                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponse<BasketResponse>(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Customer}")]
        public ApiResponse AddToBasket(int productId)
        {
            try
            {
                var userIdClaims = User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
                if (userIdClaims == null)
                {
                    return new ApiResponse(WarningType.LoginFail);
                }

                var userId = Convert.ToInt32(userIdClaims.Value);
                var response = _basketService.Insert(productId, userId);
                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        [HttpDelete("{ProductId}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Customer}")]
        public ApiResponse DeleteBasketItem(int ProductId)
        {

            var userIdClaims = User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
            var userId = Convert.ToInt32(userIdClaims.Value);
            var response = _basketService.DeleteBasketItem(userId,ProductId);
            return response;
        }
    }
}

