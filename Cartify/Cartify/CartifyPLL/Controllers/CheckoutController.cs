using CartifyBLL.Helper;
using CartifyBLL.Services.CartService.Abstraction;
using CartifyBLL.Services.CheckoutService.Abstraction;
using CartifyBLL.Services.UserServices;
using CartifyBLL.ViewModels.Account;
using CartifyBLL.ViewModels.Checkout;
using CartifyDAL.Repo.Abstraction;
using CartifyDAL.Repo.productRepo.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICheckoutService _checkoutService;
    private readonly IUserService _userService;
    private readonly IOrderRepo _orderRepo;
    private readonly IOrderItemRepo _orderItemRepo;
    private readonly IProductRepo _productRepo;

    public CheckoutController(
        ICartService cartService,
        ICheckoutService checkoutService,
        IUserService userService,
        IOrderRepo orderRepo,
        IOrderItemRepo orderItemRepo, IProductRepo productRepo)
    {
        _cartService = cartService;
        _checkoutService = checkoutService;
        _userService = userService;
        _orderRepo = orderRepo;
        _orderItemRepo = orderItemRepo;
        _productRepo= productRepo;
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
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, message = "Please log in first." });

            return RedirectToAction("Login", "Account");
        }

        // Get current cart
        var (cartVM, cartError) = _cartService.GetUserCart(userId);
        if (!string.IsNullOrEmpty(cartError) || cartVM.IsEmpty)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, message = "Your cart is empty." });

            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }

        model.Cart = cartVM;

        // ✅ Added validation: Ensure an address is selected
        if (!model.SelectedAddressId.HasValue)
        {
            var errorMsg = "Please select a shipping address.";
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, message = errorMsg });

            ModelState.AddModelError("SelectedAddressId", errorMsg);
            var userProfile = await _userService.GetUserProfileAsync(userId);
            model.UserAddresses = userProfile?.Addresses ?? new List<AddressVM>();
            return View("Index", model);
        }

        var (isValid, validationError) = _checkoutService.ValidateCheckout(model);
        if (!isValid)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, message = validationError });

            ModelState.AddModelError(string.Empty, validationError ?? "Please check your entries.");
            var userProfile = await _userService.GetUserProfileAsync(userId);
            model.UserAddresses = userProfile?.Addresses ?? new List<AddressVM>();
            return View("Index", model);
        }

        try
        {
            var (confirmation, error) = _checkoutService.ProcessOrder(model, userId);
            if (confirmation == null)
            {
                var safeError = string.IsNullOrWhiteSpace(error) ? "Failed to process order" : error;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = safeError });

                TempData["Error"] = safeError;
                var userProfile = await _userService.GetUserProfileAsync(userId);
                model.UserAddresses = userProfile?.Addresses ?? new List<AddressVM>();
                return View("Index", model);
            }

            // ✅ Reduce product quantity in database
            foreach (var item in confirmation.OrderItems)
            {
                await _productRepo.ReduceStockAsync(item.ProductId, item.Quantity);
            }

            // ✅ AJAX request → return orderId for JS redirect
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, orderId = confirmation.OrderId });

            TempData["Success"] = "Your order has been placed successfully!";
            return RedirectToAction("Index", "Orders");


        }
        catch (Exception ex)
        {
            var innerMessage = ex.InnerException?.Message;
            var fullError = $"{ex.Message} {(innerMessage ?? string.Empty)}";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, message = fullError });

            TempData["Error"] = fullError;
            var userProfile = await _userService.GetUserProfileAsync(userId);
            model.UserAddresses = userProfile?.Addresses ?? new List<AddressVM>();
            return View("Index", model);
        }
    }


    public IActionResult Confirmation(int id)
    {
        var userId = User.GetUserId();
        if (string.IsNullOrEmpty(userId))
            return RedirectToAction("Login", "Account");

        var (confirmation, error) = _checkoutService.GetOrderConfirmation(id, userId);
        if (confirmation == null)
        {
            TempData["Error"] = error ?? "Order not found.";
            return RedirectToAction("Index", "Home");
        }

        if (Request.Query.ContainsKey("fromAjax") || TempData["Success"] != null)
        {
            ViewBag.ShowToast = true;
            ViewBag.ToastMessage = TempData["Success"] ?? "Your order has been placed successfully!";
        }

        return View("Confirmation", confirmation);
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