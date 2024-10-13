using FrietGeenPatatZaakMVC.Models;
using Microsoft.CodeAnalysis;

namespace FrietGeenPatatZaakMVC.View_Model
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product)
        {
            ProductId = product.ProductId;
            Name = product.Name;
            Price = product.Price;
            Status = product.Status;
            CategoryId = product.CategoryId;
            Category = product.Category;
            OrderDetails = product.OrderDetails;
        }
        public ProductViewModel(int productId, string name, decimal price, bool status, int? categoryId, Category? category, ICollection<OrderDetail> orderDetails)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Status = status;
            CategoryId = categoryId;
            Category = category;
            OrderDetails = orderDetails;
        }

        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public bool Status { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
