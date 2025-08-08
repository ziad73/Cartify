namespace CartifyBLL.ViewModels.Cart;

public class CartVm
{
    public List<CartItemVm> Items { get; set; } = new List<CartItemVm>();
    public decimal Total => Items.Sum(item => item.Subtotal);
    public int ItemCount => Items.Sum(item => item.Quantity);
}

