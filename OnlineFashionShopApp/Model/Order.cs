using System;

namespace OnlineFashionShopApp.Models;

    public partial class Order
    {
    public string ShippingAddress { get; set; }
    public decimal GrandTotal { get; set; }
    public string CartNumber { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public string CVV { get; set; }
    public List<PaymentProduct> OrderedProducts { get; set; }
}
public partial class Orders
{
    public string ShippingAddress { get; set; }
    public decimal GrandTotal { get; set; }
    public string CartNumber { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public string CVV { get; set; }
    public string OrderedProducts { get; set; }
}

public class Products
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; } // Ensure Price property is defined
    public int Quantity { get; set; } // Ensure Quantity property is defined
}
