using System;
using System.Collections.Generic;

namespace ETicaret_Infrastructure.Data.Entities;

public partial class DeliveryCompany
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Deliverer> Deliverers { get; set; } = new List<Deliverer>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
