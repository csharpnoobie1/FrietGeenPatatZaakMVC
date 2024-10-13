using System;
using System.Collections.Generic;

namespace FrietGeenPatatZaakMVC.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
