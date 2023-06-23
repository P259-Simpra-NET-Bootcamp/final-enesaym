using AutoMapper;
using Serilog;
using SimShop.Base;
using SimShop.Base.Constants;
using SimShop.Data;
using SimShop.Data.Domain;
using SimShop.Data.UnitOfWork;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public class OrderService:IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    #region Create Order
    public ApiResponse CreateOrder(int userId,int couponCode)
    {
        // Kullanıcının sepetini al
        var basket = _unitOfWork.Repository<Basket>()
            .GetAllWithInclude("BasketItems.Product")
            .FirstOrDefault(b => b.UserId == userId);

        if (basket == null)
        {
            Log.Warning(WarningType.NotFound);
            return new ApiResponse(WarningType.BasketNotFound);
        }

        var outOfStockItems = basket.BasketItems.Where(item => item.Product.Stock < item.Quantity).ToList();
        if (outOfStockItems.Any())
        {
            var outOfStockItemNames = string.Join(", ", outOfStockItems.Select(item => item.Product.Name));
            return new ApiResponse($"Stokta olmayan ürünler var: {outOfStockItemNames}");
        }
        //kullanıcıyı al
        var user = _unitOfWork.Repository<User>().GetById(userId);
        var wallet = user.Wallet;

        //varsa kuponu al 
        var couponAmount= CouponCheck(couponCode,userId);

        //kupon kodu girildiyse ve yanlıssa hata döndürür
        if (couponCode != 0 && couponAmount == 0)
        {
            // Hatalı kupon kodu 
            return new ApiResponse("Geçersiz kupon kodu.");
        }


        //kupon ve puan kullanımı sonrası her urunun kazandıracagı puanı hesaplar
        decimal productCount = basket.BasketItems.Select(item => item.ProductId).Distinct().Count();
        decimal pointByProduct = (wallet+couponAmount) / productCount;
        

        decimal basketTotalAmount=ItemTotalAmount(basket);
        decimal amountPaid = PaidAmount(wallet,basketTotalAmount,couponAmount,userId);
        decimal usedPoint=basketTotalAmount-(amountPaid+couponAmount);
        decimal earnedPoints = ItemTotalPoints(basket, pointByProduct);
        if(earnedPoints < 0)
        {
            earnedPoints = 0;
        }
        user.Wallet+= earnedPoints;
        //_unitOfWork.Repository<User>().Update(user);
        Random random = new Random();

        var orderItems = new List<OrderItem>();
        foreach (var basketItem in basket.BasketItems)
        {
            var orderItem = new OrderItem
            {
                ProductId = basketItem.ProductId,
                Quantity = basketItem.Quantity,
                Price = basketItem.Product.Price
            };
            orderItems.Add(orderItem);
        }

        var order = new Order
        {
            CreatedAt = DateTime.Now,
            CreatedBy = Environment.MachineName,
            OrderDate=DateTime.Now,
            UserId = userId,
            UsedPoints = usedPoint,
            OrderNumber = random.Next(100000000, 1000000000),
            TotalAmount = basketTotalAmount,
            PaidAmount = amountPaid,
            EarnedPoints = earnedPoints,
            UsedCoupon = couponAmount,
            OrderItems=orderItems
        };
        //ürün stoklarını guncelle
        foreach (var orderItem in orderItems)
        {
            var product = _unitOfWork.Repository<Product>().GetById(orderItem.ProductId);
            product.Stock -= orderItem.Quantity;
            _unitOfWork.Repository<Product>().Update(product);
        }
        //kullaniciya ait sepeti sil
        if (basket != null)
        {
            foreach (var basketItem in basket.BasketItems)
            {
                _unitOfWork.Repository<BasketItem>().Delete(basketItem);
            }
            _unitOfWork.Repository<Basket>().Delete(basket);
        }
        _unitOfWork.Repository<User>().Update(user);
        _unitOfWork.Repository<Order>().Insert(order);
        _unitOfWork.Complete();
        return new ApiResponse();
    }
    public decimal CouponCheck(int couponCode,int userId)
    {
        if (couponCode == 0)
        {
            return 0;
        }
        else
        {
            var coupon = _unitOfWork.Repository<Coupon>().GetAll()
           .FirstOrDefault(c => c.UserId == userId && c.Code == couponCode 
            && c.IsActive==true && c.ValidTo >= DateTime.Now);
            if (coupon != null)
            {
                // Kupon miktarını döndür
                coupon.IsActive = false;
                _unitOfWork.Repository<Coupon>().Update(coupon);
                return coupon.DiscountAmount;
               
            }
            else
            {
                return 0;
            }
        }
    }

    //sepetteki ürünlerin toplam fiyatını hesaplar
    public decimal ItemTotalAmount(Basket basket)
    {
        decimal totalAmount = 0;
        foreach (var basketItem in basket.BasketItems)
        {
            decimal itemTotalAmount = basketItem.Quantity * basketItem.Product.Price;
            totalAmount += itemTotalAmount;
        }
        return totalAmount;
    }
    //odenecek tutarı hesaplar
    public decimal PaidAmount(decimal wallet, decimal basketTotalAmount,decimal couponAmount,int userId)
    {
        decimal paidAmount = 0;
        if (couponAmount >= basketTotalAmount)
        {
            return 0;
        }
        else
        {
            basketTotalAmount -= couponAmount;
        }
        if (wallet >= basketTotalAmount)
        {
            wallet -= basketTotalAmount;
        }
        else
        {
            paidAmount=basketTotalAmount-wallet;
            wallet = 0;
        }
        var user = _unitOfWork.Repository<User>().GetById(userId);
        user.Wallet =wallet;
        _unitOfWork.Repository<User>().Update(user);
        return paidAmount;
    }

    //sepetteki ürünlerin toplam kazandırdıgı puanı hesaplar
    public decimal ItemTotalPoints(Basket basket,decimal pointByProduct)
    {
        decimal totalPoint = 0;
        foreach (var basketItem in basket.BasketItems)
        {
            decimal itemTotalAmount = basketItem.Quantity * basketItem.Product.Price;
            decimal itemEarnedPoints = CalculateEarnedPoints(itemTotalAmount-pointByProduct, basketItem.Product.PointPercentage, basketItem.Product.MaxPoint);
            totalPoint += itemEarnedPoints;
        }
        return totalPoint;
    }


    public decimal CalculateEarnedPoints(decimal totalAmount, decimal pointPercentage, decimal maxPoint)
    {
        // Ürünün kazandırdıgı puan miktarını hesaplar
        decimal earnedPoints = totalAmount * pointPercentage/100;

        // Ürünün maximum puanını kontrol et
        if (earnedPoints > maxPoint)
        {
            earnedPoints = maxPoint;
        }
        return earnedPoints;
    }
    #endregion

    #region Get Orders
    public ApiResponse<List<OrderResponse>> GetAllUserOrders(int userId)
    {
        try
        {
            var orders = _unitOfWork.Repository<Order>()
                .GetAllWithInclude("OrderItems.Product")
                .Where(o => o.UserId == userId)
                .ToList();

            var orderResponses = _mapper.Map<List<OrderResponse>>(orders);
            return new ApiResponse<List<OrderResponse>>(orderResponses);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<OrderResponse>>(ex.Message);
        }
    }

#endregion

}

