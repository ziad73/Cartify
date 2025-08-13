namespace CartifyBLL.ViewModels.Cart;

public class CartItemVm
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImageUrl { get; set; }
    public double ProductPrice { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice => ProductPrice * Quantity;
    public string Category { get; set; }
    public int StockQuantity { get; set; }
    public bool IsAvailable => StockQuantity > 0;
}