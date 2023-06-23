using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Schema;

public class TokenResponse
{
    public DateTime ExpireTime { get; set; }
    public string AccessToken { get; set; }
    public string UserName { get; set; }
}

