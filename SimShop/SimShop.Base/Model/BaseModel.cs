using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Base;

public class BaseModel
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}
