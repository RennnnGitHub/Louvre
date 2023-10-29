namespace OnlineFashionShopApp.Models;

public class CartItem
{
    public string productId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ProductImageBase64 { get; set; }
}
public class Product
{
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }

    public string ProductImageBase64 { get; set; }
    // Add other properties as needed
}
