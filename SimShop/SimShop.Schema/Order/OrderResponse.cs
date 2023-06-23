using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Schema;

public class OrderResponse
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal UsedPoints { get; set; }
    public decimal UsedCoupon { get; set; }
    public decimal EarnedPoints { get; set; }
    public List<OrderItemResponse> OrderItems { get; set; }
}
