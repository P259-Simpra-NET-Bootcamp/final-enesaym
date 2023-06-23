using AutoMapper;
using SimShop.Base.Constants;
using SimShop.Base;
using SimShop.Data.UnitOfWork;
using SimShop.Data;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimShop.Data.Domain;
using Serilog;

namespace SimShop.Operation;

public class AdminService :IAdminService 
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    public AdminService(IUnitOfWork unitOfWork, IMapper mapper) 
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    //sistemdeki tüm kullanıcıları getirir
    public ApiResponse<List<UserResponse>> GetAllUsers()
    {
        try
        {
            var users = unitOfWork.Repository<User>().GetAll();
            var userResponses = mapper.Map<List<User>, List<UserResponse>>(users);
            return new ApiResponse<List<UserResponse>>(userResponses);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<UserResponse>>(ex.Message);
        }
    }


    //admin rolune sahip kullanıcı ekler
    public ApiResponse Insert(UserRequest request)
    {
        var exist = unitOfWork.Repository<User>().
            Where(x => x.UserName.Equals(request.UserName)).ToList();

        if (exist.Any())
        {
            return new ApiResponse(WarningType.UserNameUsed);
        }

        try
        {
            request.Password = CreateMD5(request.Password);
            var entity = mapper.Map<UserRequest, User>(request);
            entity.CreatedAt = DateTime.UtcNow;
            entity.LastName = request.Surname;
            entity.CreatedBy = entity.UserName;
            entity.Status = true;
            entity.Role = UserRoles.Admin;

            unitOfWork.Repository<User>().Insert(entity);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    //kullaniciyi gunceller
    public ApiResponse Update(int userId, AdminUserRequest request)
    {
        var user = unitOfWork.Repository<User>().GetById(userId);
        if (user == null)
        {
            return new ApiResponse(WarningType.UserNotFound);
        }
        var existingUser = unitOfWork.Repository<User>().Where(u => u.UserName.Equals(request.UserName) && u.Id != userId).FirstOrDefault();
        if (existingUser != null)
        {
            return new ApiResponse(WarningType.UserNameUsed);
        }
        try
        {
            user.Id = user.Id;
            user.Name = request.Name;
            user.LastName = request.Surname;
            user.Password = CreateMD5(request.Password);
            user.UserName = request.UserName;
            user.Email = request.Email;
            unitOfWork.Repository<User>().Update(user);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    //kullanici silmeden once ona ait sepet ve siparişleri siler daha sonra kullaniciyi siler
    public ApiResponse Delete(int userId)
    {
        var user = unitOfWork.Repository<User>().GetById(userId);
        if (user == null)
        {
            return new ApiResponse(WarningType.InvalidUser);
        }
        try
        {
            var orders = unitOfWork.Repository<Order>().Where(o => o.UserId == userId).ToList();
            foreach (var order in orders)
            {
                unitOfWork.Repository<Order>().Delete(order);
            }
            var basket = unitOfWork.Repository<Basket>()
            .GetAllWithInclude("BasketItems.Product")
            .FirstOrDefault(b => b.UserId == userId);
            if (basket != null)
            {
                foreach (var basketItem in basket.BasketItems)
                {
                    unitOfWork.Repository<BasketItem>().Delete(basketItem);
                }
                unitOfWork.Repository<Basket>().Delete(basket);
            }
            unitOfWork.Repository<User>().Delete(user);

            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.DeleteError);
            return new ApiResponse(ex.Message);
        }
    }

    private string CreateMD5(string input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();

        }
    }
}
