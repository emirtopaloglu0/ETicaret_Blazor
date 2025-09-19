using System;
using System.Collections.Generic;

namespace ETicaret_Infrastructure.Data.Entities;

public partial class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int ShopId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Stock { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Shop Shop { get; set; } = null!;
}
