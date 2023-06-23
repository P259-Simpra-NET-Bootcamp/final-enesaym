using SimShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Schema;

public class ProductRequest
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
    public int MaxPoint { get; set; }
    public int PointPercentage { get; set; }

}
