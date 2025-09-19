using System;
using System.Collections.Generic;

namespace ETicaret_Infrastructure.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Deliverer? Deliverer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ShopUser> ShopUsers { get; set; } = new List<ShopUser>();
}
