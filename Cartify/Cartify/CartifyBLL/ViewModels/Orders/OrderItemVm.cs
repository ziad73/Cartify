namespace CartifyBLL.ViewModels.Orders;

public class OrderItemVm
{
    public string ProductName { get; set; }
    public int OrderItemId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
    public double ItemTotal { get; set; }
}