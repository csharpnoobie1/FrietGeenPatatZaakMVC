using System;
using System.Collections.Generic;

namespace FrietGeenPatatZaakMVC.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public bool Status { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
