namespace CartifyBLL.ViewModels.Orders;

public class OrderDetailsVm
{
    
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShippingMethod { get; set; }
    public string TrackingNumber { get; set; }
    public double ShippingCost { get; set; }
    public double Tax { get; set; }
    public double TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    public string PaymentStatus { get; set; }
    public List<OrderItemVm> OrderItems { get; set; }
}