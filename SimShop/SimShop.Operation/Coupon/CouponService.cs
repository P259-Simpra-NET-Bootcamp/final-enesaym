using AutoMapper;
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

public class CouponService:ICouponService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CouponService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    public ApiResponse<List<CouponResponse>> GetUserCoupons(int userId)
    {
        try
        {
            var coupons = _unitOfWork.Repository<Coupon>().Where(c => c.UserId == userId).ToList();

            if (coupons.Count == 0)
            {
                // Kupon bulunamadı
                return new ApiResponse<List<CouponResponse>>(WarningType.NotFound);
            }

            var couponResponses = _mapper.Map<List<Coupon>, List<CouponResponse>>(coupons);

            return new ApiResponse<List<CouponResponse>>(couponResponses);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CouponResponse>>(ex.Message);
        }
    }

    public ApiResponse CouponCreate(CouponRequest request)
    {
        try
        {
            var existingUser = _unitOfWork.Repository<User>().GetById(request.UserId);
            if (existingUser == null)
            {
                return new ApiResponse(WarningType.UserNotFound);
            }
            Random random = new Random();
            var entity = _mapper.Map<CouponRequest, Coupon>(request);
            entity.CreatedAt = DateTime.Now;
            entity.CreatedBy = Environment.MachineName;
            entity.Code = random.Next(100000000, 1000000000);
            entity.IsActive = true;
            _unitOfWork.Repository<Coupon>().Insert(entity);
            _unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }



}
