using Microsoft.EntityFrameworkCore;
using SimShop.Base;
using SimShop.Data.Domain;
using SimShop.Data;
using SimShop.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimShop.Base.Constants;
using SimShop.Schema;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Serilog;

namespace SimShop.Operation;

public class BasketService :IBasketService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BasketService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }

    #region Insert to Basket
    public ApiResponse Insert(int productId,int userId)
    {
        try
        {
            var user = _unitOfWork.Repository<User>().GetById(userId);
            if (user == null)
            {
                return new ApiResponse(WarningType.UserNotFound);
            }   
            var product = _unitOfWork.Repository<Product>().GetById(productId);
            if (product == null)
            {
                return new ApiResponse(WarningType.ProductNotFound);
            }
            // Kullanıcının sepetini kontrol et veya oluştur
            var basket = _unitOfWork.Repository<Basket>().Where(b => b.UserId == userId).FirstOrDefault();
            if (basket == null)
            {
                basket = new Basket
                {
                    UserId = userId,
                    User = user,
                    CreatedAt=DateTime.UtcNow,
                    CreatedBy=Environment.MachineName,
                };
                _unitOfWork.Repository<Basket>().Insert(basket);
                _unitOfWork.Complete();
            }
            // Ürünü sepete ekle
            var existingBasketItem = _unitOfWork.Repository<BasketItem>()
                .Where(item => item.BasketId == basket.Id && item.ProductId == productId)
                .FirstOrDefault();
            if (existingBasketItem != null)
            {
                // Ürün zaten sepete eklenmişse, adeti güncelle
                existingBasketItem.Quantity += 1;

                _unitOfWork.Repository<BasketItem>().Update(existingBasketItem);
                _unitOfWork.Complete();
            }
            else
            {
                // Yeni ürünse, sepete ekle
                var newBasketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Environment.MachineName,
                };
                _unitOfWork.Repository<BasketItem>().Insert(newBasketItem);
                _unitOfWork.Complete();
            }
                    
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }
    #endregion

    #region Get Basket
    public ApiResponse<BasketResponse> GetBasket(int userId)
    {
        try
        {
            var basket = _unitOfWork.Repository<Basket>()
                .GetAllWithInclude("BasketItems.Product")
                .FirstOrDefault(b => b.UserId == userId);
            if (basket == null)
            {
                // Sepet bulunamadı
                return new ApiResponse<BasketResponse>(WarningType.BasketNotFound);
            }
            var basketResponse = _mapper.Map<BasketResponse>(basket);
            return new ApiResponse<BasketResponse>(basketResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<BasketResponse>(ex.Message);
        }
    }
    #endregion

    public ApiResponse DeleteBasketItem(int userId,int productId)
    {
        try
        {
            var basket = _unitOfWork.Repository<Basket>().Where(b => b.UserId == userId).FirstOrDefault();
            if (basket == null)
            {
                return new ApiResponse(WarningType.BasketNotFound);
            }

            var existingBasketItem = _unitOfWork.Repository<BasketItem>()
                .Where(item => item.BasketId == basket.Id && item.ProductId == productId)
                .FirstOrDefault();

            if (existingBasketItem == null)
            {
                return new ApiResponse(WarningType.BasketItemNotFound);
            }

            if(existingBasketItem.Quantity == 1)
            {
                _unitOfWork.Repository<BasketItem>().Delete(existingBasketItem);
                _unitOfWork.Complete();
            }
            else 
            {
                existingBasketItem.Quantity -= 1;
                _unitOfWork.Repository<BasketItem>().Update(existingBasketItem);
                _unitOfWork.Complete();
            }
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.DeleteError);
            return new ApiResponse(ex.Message);
        }
    }



}
