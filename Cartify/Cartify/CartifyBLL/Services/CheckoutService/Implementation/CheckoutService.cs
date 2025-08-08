using CartifyBLL.Services.CartService;
using CartifyBLL.ViewModels.Checkout;
using CartifyDAL.Entities.order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Cartify.DAL.DataBase;

namespace CartifyBLL.Services.CheckoutService
{
    public class CheckoutService : ICheckoutService
    {
        private readonly CartifyDbContext _context;
        private readonly ICartService _cartService;
        private readonly ILogger<CheckoutService> _logger;

        public CheckoutService(CartifyDbContext context, ICartService cartService, ILogger<CheckoutService> logger)
        {
            _context = context;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<CheckoutVm> InitializeCheckoutAsync(string userName)
        {
            try
            {
                var cart = await _cartService.GetUserCartAsync(userName);
                if (!cart.Items.Any())
                {
                    throw new InvalidOperationException("Cart is empty");
                }

                return new CheckoutVm
                {
                    CartItems = cart.Items.Select(item => new CheckoutItemVM
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = (double)item.Price,
                        Discount = 0.0 // Default, adjust if dynamic discounts are implemented
                    }).ToList(),
                    SubTotal = (double)cart.Total,
                    ShippingCost = 50.0,
                    Tax = 260.0,
                    OrderStatus = "Pending",
                    ShippingMethod = "Standard"
                };
            }
            catch (InvalidOperationException)
            {
                throw; // Let controller handle empty cart
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing checkout for user {UserName}", userName);
                throw;
            }
        }

        public async Task<(bool success, string orderId, string message)> ProcessCheckoutAsync(CheckoutVm model, string userName)
        {
            try
            {
                // Validate payment fields
                if (!IsValidCardNumber(model.CardNumber) ||
                    string.IsNullOrWhiteSpace(model.CardHolderName) ||
                    !IsValidExpiryDate(model.ExpiryDate) ||
                    !IsValidCVV(model.CVV))
                {
                    _logger.LogWarning("Invalid payment details for user {UserName}", userName);
                    return (false, null, "Invalid payment details");
                }

                var cart = await _cartService.GetUserCartAsync(userName);
                if (!cart.Items.Any())
                {
                    _logger.LogWarning("Empty cart for user {UserName} during checkout", userName);
                    return (false, null, "Your cart is empty");
                }

                var order = new Order(
                    orderStatus: model.OrderStatus,
                    shippingMethod: model.ShippingMethod,
                    trackingNumber: model.TrackingNumber,
                    shippingCost: model.ShippingCost,
                    tax: model.Tax,
                    createdBy: userName
                )
                {
                    UserId = userName
                };

                foreach (var item in model.CartItems)
                {
                    order.OrderItems.Add(new OrderItem(
                      
                        quantity: item.Quantity,
                        price: item.Price,
                        discount: item.Discount,
                        createdBy: userName
                    ));
                }

                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                // Clear the cart
                await _cartService.ClearCartAsync(userName);

                return (true, order.OrderId.ToString(), "Order placed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing checkout for user {UserName}", userName);
                return (false, null, "An error occurred during checkout");
            }
        }

        public async Task<double> CalculateShippingCostAsync(string country, string postalCode)
        {
            try
            {
                // Static shipping cost
                // TODO: Implement dynamic shipping logic if needed
                return await Task.FromResult(50.0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating shipping cost for country {Country}, postal code {PostalCode}", country, postalCode);
                throw;
            }
        }

        private bool IsValidCardNumber(string cardNumber)
        {
            return !string.IsNullOrWhiteSpace(cardNumber) && cardNumber.Length == 16 && long.TryParse(cardNumber, out _);
        }

        private bool IsValidExpiryDate(string expiryDate)
        {
            if (string.IsNullOrWhiteSpace(expiryDate) || !System.Text.RegularExpressions.Regex.IsMatch(expiryDate, @"^(0[1-9]|1[0-2])\/([0-9]{2})$"))
                return false;

            var parts = expiryDate.Split('/');
            if (!int.TryParse(parts[0], out int month) || !int.TryParse(parts[1], out int year))
                return false;

            var currentYear = DateTime.Now.Year % 100;
            var currentMonth = DateTime.Now.Month;
            return year > currentYear || (year == currentYear && month >= currentMonth);
        }

        private bool IsValidCVV(string cvv)
        {
            return !string.IsNullOrWhiteSpace(cvv) && (cvv.Length == 3 || cvv.Length == 4) && int.TryParse(cvv, out _);
        }
    }
}