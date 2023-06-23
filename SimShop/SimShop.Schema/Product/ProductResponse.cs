using SimShop.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Schema;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
    public int MaxPoint { get; set; }
    public int PointPercentage { get; set; }
    public int CategoryId { get; set; }
}
