using CartifyBLL.ViewModels.Checkout;

namespace CartifyBLL.Services.CheckoutService.Abstraction;

public interface ICheckoutService
{
    (CheckoutVm, string?) GetCheckoutData(string userId);
    (bool, string?) ValidateCheckout(CheckoutVm model);
    (OrderConfirmationVm, string?) ProcessOrder(CheckoutVm model, string userId);
    (OrderConfirmationVm, string?) GetOrderConfirmation(int orderId, string userId);
}