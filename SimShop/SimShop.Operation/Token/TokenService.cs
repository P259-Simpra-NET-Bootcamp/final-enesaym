using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimShop.Base;
using SimShop.Data.UnitOfWork;
using SimShop.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SimShop.Schema;
using SimShop.Base.Constants;

namespace SimShop.Operation;

public class TokenService : ITokenService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtConfig jwtConfig;

        public TokenService(IUnitOfWork unitOfWork,IOptionsMonitor<JwtConfig> jwtConfig)
        {
            this.unitOfWork = unitOfWork;
            this.jwtConfig = jwtConfig.CurrentValue;
        }

        public ApiResponse<TokenResponse> GetToken(TokenRequest request)
        {
            if (request is null)
            {
                return new ApiResponse<TokenResponse>(WarningType.RequestNull);
            }
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return new ApiResponse<TokenResponse>(WarningType.RequestNull);
            }

            request.UserName = request.UserName.Trim().ToLower();
            request.Password = request.Password.Trim();

            var user = unitOfWork.Repository<User>().Where(x => x.UserName.Equals(request.UserName)).FirstOrDefault();
            if (user is null)
            {
                
                return new ApiResponse<TokenResponse>(WarningType.InvalidUser);
            }

            if (user.Status != true)
            {
              
                return new ApiResponse<TokenResponse>(WarningType.InvalidUserStatus);
            }
           
            user.Status = true;
            unitOfWork.Repository<User>().Update(user);
            unitOfWork.Complete();
            TokenResponse response = new();
            response.UserName = request.UserName;
            response.AccessToken = Token(user);
            response.ExpireTime = DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration);

            return new ApiResponse<TokenResponse>(response);
        }

        private string Token(User user)
        {
            Claim[] claims = GetClaims(user);
            var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            var jwtToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }

        private Claim[] GetClaims(User user)
        {
            var claims = new[]
            {
            new Claim("UserId",user.Id.ToString()),
            new Claim("Role",user.Role),
            new Claim("Status",user.Status.ToString()),
            new Claim(ClaimTypes.Role,user.Role),
        };

            return claims;
        }
        //private string CreateMD5(string input)
        //{
        //    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        //    {
        //        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        //        byte[] hashBytes = md5.ComputeHash(inputBytes);

        //        return Convert.ToHexString(hashBytes).ToLower();

        //    }
        //}
    }

