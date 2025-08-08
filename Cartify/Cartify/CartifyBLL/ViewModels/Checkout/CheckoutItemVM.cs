namespace CartifyBLL.ViewModels.Checkout;

public class CheckoutItemVM
{
   
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Subtotal => (Price * Quantity) - Discount;
    
}