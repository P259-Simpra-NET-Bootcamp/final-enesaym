using SimShop.Base;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public interface ITokenService
{
    ApiResponse<TokenResponse> GetToken(TokenRequest request);
}
