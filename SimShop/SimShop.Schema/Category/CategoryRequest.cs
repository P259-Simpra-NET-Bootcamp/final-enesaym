using SimShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Schema;

public class CategoryRequest
{
    //public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Tag { get; set; }

    //public List<ProductRequest> Products { get; set; } = new List<ProductRequest>();
}
