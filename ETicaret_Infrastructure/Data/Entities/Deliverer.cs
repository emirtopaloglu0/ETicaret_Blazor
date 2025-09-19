using System;
using System.Collections.Generic;

namespace ETicaret_Infrastructure.Data.Entities;

public partial class Deliverer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public virtual DeliveryCompany Company { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
