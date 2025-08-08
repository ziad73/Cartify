using CartifyBLL.ViewModels.Checkout;
using System.Threading.Tasks;

namespace CartifyBLL.Services.CheckoutService
{
    public interface ICheckoutService
    {
        Task<CheckoutVm> InitializeCheckoutAsync(string userName);
        Task<(bool success, string orderId, string message)> ProcessCheckoutAsync(CheckoutVm model, string userName);
        Task<double> CalculateShippingCostAsync(string country, string postalCode);
    }
}