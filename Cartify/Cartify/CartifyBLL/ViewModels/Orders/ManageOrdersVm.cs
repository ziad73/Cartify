namespace CartifyBLL.ViewModels.Orders;

public class ManageOrdersVm
{
    public int OrderId { get; set; }
    public string ProductName { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    public string PaymentStatus { get; set; }
    public int ItemCount { get; set; }
}