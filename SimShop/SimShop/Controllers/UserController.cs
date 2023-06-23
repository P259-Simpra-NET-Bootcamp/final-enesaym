using Microsoft.AspNetCore.Mvc;
using SimShop.Data;
using System.Threading.Tasks;
using System;
using SimShop.Data.Context;
using SimShop.Schema;
using SimShop.Base;
using SimShop.Operation;
using AutoMapper;
using SimShop.Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using SimShop.Base.Constants;
using System.Data;
using System.Collections.Generic;

namespace SimShop.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public UserController(IAdminService adminService)
        {
            this._adminService= adminService;
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public ApiResponse<List<UserResponse>> GetUsers()
        {
            var response = _adminService.GetAllUsers();
            return response;
        }

        [HttpPost("InsertUser")]
        [Authorize(Roles = UserRoles.Admin)]
        public ApiResponse Post([FromBody] UserRequest request)
        {
            var response = _adminService.Insert(request);
            return response;
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public ApiResponse UpdateUser(int id, [FromBody] AdminUserRequest request)
        {
            var response = _adminService.Update(id, request);
            return response;
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public ApiResponse DeleteProduct(int id)
        {
            var response = _adminService.Delete(id);
            return response;
        }
    }
}
