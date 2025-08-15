using System.ComponentModel.DataAnnotations;
using CartifyBLL.ViewModels.Account;
using CartifyBLL.ViewModels.Cart;

namespace CartifyBLL.ViewModels.Checkout;

public class CheckoutVm
{
    public CartVm Cart { get; set; } = new CartVm();

    [Required(ErrorMessage = "Please select a shipping address.")]
    public int? SelectedAddressId { get; set; }
        
    public List<AddressVM> UserAddresses { get; set; } = new List<AddressVM>();
        
    public AddressVM NewAddress { get; set; } = new AddressVM();
        
    public bool UseNewAddress { get; set; } = false;
        
    [Required]
    public string PaymentMethod { get; set; } = "CreditCard";
        
    // Payment details (you might want to use a separate payment processor)
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
    public string CVV { get; set; }
        
    public string OrderNotes { get; set; }
        
    // Order summary
    public double SubTotal => Cart.SubTotal;
    public double Tax => SubTotal * 0.1; // 10% tax example
    public double ShippingCost => SubTotal > 100 ? 0 : 15; // Free shipping over $100
    public double Total => SubTotal + Tax + ShippingCost;
}