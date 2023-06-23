using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimShop.Base;
using SimShop.Base.Constants;
using SimShop.Operation;
using SimShop.Schema;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace SimShop.Service;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ITokenService tokenService;
    private readonly IUserService userService;


    public LoginController(ITokenService tokenService, IUserService userService)
    {
        this.tokenService = tokenService;
        this.userService = userService;
    }
    [HttpGet]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Customer}")]
    public ApiResponse<UserResponse> GetUserProfile()
    {
        try
        {
            var userIdClaims = User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
            if (userIdClaims == null)
            {
                throw new Exception(WarningType.LoginFail);
            }

            var userId = Convert.ToInt32(userIdClaims.Value);
            var response = userService.GetUserProfile(userId);
            return response;
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserResponse>(ex.Message);
        }
    }

    [HttpPost("SignIn")]
    public ApiResponse<TokenResponse> Post([FromBody] TokenRequest request)
    {
        return tokenService.GetToken(request);
    }


    [HttpPost("SignUp")]
    public ApiResponse Post([FromBody] UserRequest request)
    {
        var response = userService.Insert(request);
        return response;
    }

}
