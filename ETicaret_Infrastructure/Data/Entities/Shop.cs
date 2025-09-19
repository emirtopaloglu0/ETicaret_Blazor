using System;
using System.Collections.Generic;

namespace ETicaret_Infrastructure.Data.Entities;

public partial class Shop
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ShopUser> ShopUsers { get; set; } = new List<ShopUser>();
}
