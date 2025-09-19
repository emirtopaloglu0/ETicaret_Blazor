using System;
using System.Collections.Generic;

namespace ETicaret_Infrastructure.Data.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public DateTime? DeliveryDate { get; set; }

    public int DeliveryCompanyId { get; set; }

    public virtual DeliveryCompany DeliveryCompany { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
