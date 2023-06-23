using AutoMapper;
using Serilog;
using SimShop.Base;
using SimShop.Base.Constants;
using SimShop.Data;
using SimShop.Data.Domain;
using SimShop.Data.UnitOfWork;
using SimShop.Operation;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public class UserService : BaseService<User, UserRequest, UserResponse>, IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public ApiResponse<UserResponse> GetUserProfile(int userId)
    {
        try
        {
            var user = unitOfWork.Repository<User>()
                .Where(b => b.Id == userId).FirstOrDefault();
            if (user == null)
            {
                // Sepet bulunamadı
                return new ApiResponse<UserResponse>(WarningType.UserNotFound);
            }
            var userResponse = mapper.Map<UserResponse>(user);
            return new ApiResponse<UserResponse>(userResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserResponse>(ex.Message);
        }
    }

    public override ApiResponse Insert(UserRequest request)
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
            entity.Wallet = 0;
            entity.Role = UserRoles.Customer;

            unitOfWork.Repository<User>().Insert(entity);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            Log.Error(ex, LogType.InsertError);
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
