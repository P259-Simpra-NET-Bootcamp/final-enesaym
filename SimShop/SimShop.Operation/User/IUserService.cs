using SimShop.Base;
using SimShop.Data;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public interface IUserService : IBaseService<User, UserRequest, UserResponse>
{
    ApiResponse<UserResponse> GetUserProfile(int userId);
}
