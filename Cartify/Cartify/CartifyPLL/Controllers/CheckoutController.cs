using CartifyBLL.Services.CheckoutService;
using CartifyBLL.ViewModels.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartifyWeb.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(
            ICheckoutService checkoutService,
            ILogger<CheckoutController> logger)
        {
            _checkoutService = checkoutService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _checkoutService.InitializeCheckoutAsync(User.Identity.Name);
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Cart is empty for user {UserName}", User.Identity.Name);
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading checkout page for user {UserName}", User.Identity.Name);
                TempData["Error"] = "Unable to proceed to checkout";
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(CheckoutVm model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            try
            {
                var (success, orderId, message) = await _checkoutService.ProcessCheckoutAsync(model, User.Identity.Name);

                if (!success)
                {
                    ModelState.AddModelError("", message);
                    return View("Index", model);
                }

                return RedirectToAction("Confirmation", new { orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing checkout for user {UserName}", User.Identity.Name);
                ModelState.AddModelError("", "An error occurred during checkout");
                return View("Index", model);
            }
        }

        public IActionResult Confirmation(string orderId)
        {
            return View(orderId);
        }

        [HttpGet]
        public async Task<IActionResult> GetShippingCost(string country, string postalCode)
        {
            try
            {
                var cost = await _checkoutService.CalculateShippingCostAsync(country, postalCode);
                return Json(new { success = true, cost });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating shipping cost for user {UserName}", User.Identity.Name);
                return Json(new { success = false, message = "Unable to calculate shipping cost" });
            }
        }
    }
}