using SimShop.Base;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public interface IAdminService
{
    ApiResponse Insert(UserRequest request);
    ApiResponse Update(int userId, AdminUserRequest request);
    ApiResponse Delete(int userId);
    ApiResponse<List<UserResponse>> GetAllUsers();
}
