using System.ComponentModel.DataAnnotations;

namespace CartifyBLL.ViewModels.Checkout
{
    public class CheckoutVm
    {
        // Personal Information
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        // Shipping Address
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        // Payment Information
        [Required(ErrorMessage = "Card number is required")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid card number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Card holder name is required")]
        public string CardHolderName { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Invalid expiry date (MM/YY)")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Invalid CVV")]
        public string CVV { get; set; }

        // Order Summary
        [Required(ErrorMessage = "Shipping method is required")]
        public string ShippingMethod { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public string TrackingNumber { get; set; }
        public double SubTotal { get; set; }
        public double ShippingCost { get; set; }
        public double Tax { get; set; } = 260.0;
        public double Total => SubTotal + ShippingCost + Tax;
        public List<CheckoutItemVM> CartItems { get; set; } = new List<CheckoutItemVM>();
    }

 
}