using CartifyBLL.ViewModels.Cart;

namespace CartifyBLL.ViewModels.Checkout;

public class OrderConfirmationVm
{
    public int OrderId { get; set; }
    public string OrderNumber => $"ORD-{OrderId:D6}";
    public DateTime OrderDate { get; set; }
    public double OrderTotal { get; set; }
    public string CustomerEmail { get; set; }
    public string ShippingAddress { get; set; }
    public string PaymentMethod { get; set; }
    public string OrderStatus { get; set; }
    public string TrackingNumber { get; set; }
    public List<CartItemVm> OrderItems { get; set; } = new List<CartItemVm>();
}