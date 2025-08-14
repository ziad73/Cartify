namespace CartifyBLL.ViewModels.Cart;

public class CartVm
{
    public int CartId { get; set; }
    public List<CartItemVm> Items { get; set; } = new List<CartItemVm>();
    public int ItemCount => Items.Sum(i => i.Quantity);
    public double SubTotal => Items.Sum(i => i.TotalPrice);
    public double Tax { get; set; } = 0.0; // You can calculate based on your tax logic
    public double ShippingCost { get; set; } = 0.0;
    public double Total => SubTotal + Tax + ShippingCost;
    public bool IsEmpty => !Items.Any();
    public string UserId { get; set; }
}