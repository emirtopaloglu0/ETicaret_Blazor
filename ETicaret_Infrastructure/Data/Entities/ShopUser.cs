using System;
using System.Collections.Generic;

namespace ETicaret_Infrastructure.Data.Entities;

public partial class ShopUser
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ShopId { get; set; }

    public virtual Shop Shop { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
