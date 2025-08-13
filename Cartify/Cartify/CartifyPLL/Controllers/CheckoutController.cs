using CartifyBLL.Helper;
using CartifyBLL.Services.CartService.Abstraction;
using CartifyBLL.Services.CheckoutService.Abstraction;
using CartifyBLL.Services.UserServices;
using CartifyBLL.ViewModels.Account;
using CartifyBLL.ViewModels.Checkout;
using CartifyDAL.Repo.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartService _cartService;
        private readonly ICheckoutService _checkoutService;
        private readonly IUserService _userService;
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _orderItemRepo;

        public CheckoutController(
            ICartService cartService,
            ICheckoutService checkoutService,
            IUserService userService,
            IOrderRepo orderRepo,
            IOrderItemRepo orderItemRepo)
        {
            _cartService = cartService;
            _checkoutService = checkoutService;
            _userService = userService;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Get user cart
            var (cartVM, cartError) = _cartService.GetUserCart(userId);
            if (!string.IsNullOrEmpty(cartError) || cartVM.IsEmpty)
            {
                TempData["Error"] = "Your cart is empty. Please add items before checkout.";
                return RedirectToAction("Index", "Cart");
            }

            // Get user profile for addresses
            var userProfile = await _userService.GetUserProfileAsync(userId);
            
            var checkoutVM = new CheckoutVm
            {
                Cart = cartVM,
                UserAddresses = userProfile?.Addresses ?? new List<AddressVM>(),
                PaymentMethod = "CreditCard"
            };

            // Set default address if exists
            var defaultAddress = checkoutVM.UserAddresses.FirstOrDefault(a => a.IsDefault);
            if (defaultAddress != null)
            {
                checkoutVM.SelectedAddressId = defaultAddress.Id;
            }

            return View(checkoutVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder(CheckoutVm model)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Get current cart
            var (cartVM, cartError) = _cartService.GetUserCart(userId);
            if (!string.IsNullOrEmpty(cartError) || cartVM.IsEmpty)
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            // Update model with current cart data
            model.Cart = cartVM;

            if (!ModelState.IsValid)
            {
                // Reload user addresses if model is invalid
                var userProfile = await _userService.GetUserProfileAsync(userId);
                model.UserAddresses = userProfile?.Addresses ?? new List<AddressVM>();
                return View("Index", model);
            }

            try
            {
                // Add new address if needed
                if (model.UseNewAddress && model.NewAddress != null)
                {
                    var addressResult = await _userService.AddAddressAsync(userId, model.NewAddress);
                    if (!addressResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to add new address");
                        var userProfile = await _userService.GetUserProfileAsync(userId);
                        model.UserAddresses = userProfile?.Addresses ?? new List<AddressVM>();
                        return View("Index", model);
                    }
                }

                // Process the order
                var (confirmation, error) = _checkoutService.ProcessOrder(model, userId);
                if (confirmation == null)
                {
                    TempData["Error"] = error ?? "Failed to process order";
                    var userProfile = await _userService.GetUserProfileAsync(userId);
                    model.UserAddresses = userProfile?.Addresses ?? new List<AddressVM>();
                    return View("Index", model);
                }

//                TempData["Success"] = "Your order has been placed successfully!";
// return RedirectToAction("Index", "Orders");
                TempData["Success"] = "Your order has been placed successfully!";
                return RedirectToAction("Index", "Orders");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing your order. Please try again.";
                var userProfile = await _userService.GetUserProfileAsync(userId);
                model.UserAddresses = userProfile?.Addresses ?? new List<AddressVM>();
                return View("Index", model);
            }
        }

        public IActionResult Confirmation(int orderId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var (confirmation, error) = _checkoutService.GetOrderConfirmation(orderId, userId);
            if (confirmation == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(confirmation);
        }

        [HttpGet]
        public IActionResult GetShippingCost(string country, string postalCode)
        {
            // Simple shipping cost calculation - you can make this more sophisticated
            double cost = 0;
            
            switch (country?.ToUpper())
            {
                case "US":
                case "CA":
                    cost = 15.00;
                    break;
                case "GB":
                case "DE":
                case "FR":
                    cost = 25.00;
                    break;
                default:
                    cost = 35.00;
                    break;
            }

            return Json(new { success = true, cost = cost });
        }

        [HttpPost]
        public IActionResult ValidateCheckout(CheckoutVm model)
        {
            var (isValid, error) = _checkoutService.ValidateCheckout(model);
            return Json(new { valid = isValid, message = error });
        }
}