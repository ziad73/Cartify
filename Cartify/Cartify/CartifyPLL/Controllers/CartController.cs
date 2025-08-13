using System.Security.Claims;
using CartifyBLL.Helper;
using CartifyBLL.Services.CartService.Abstraction;
using CartifyBLL.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var (cartVM, error) = _cartService.GetUserCart(userId);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View(new CartVm());
            }

            return View(cartVM);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            var addToCartModel = new AddToCartVm
            {
                ProductId = productId,
                Quantity = quantity
            };

            var (success, error) = _cartService.AddToCart(userId, addToCartModel);
            
            if (success)
            {
                var (count, _) = _cartService.GetCartItemCount(userId);
                return Json(new { success = true, cartCount = count, message = "Item added to cart" });
            }

            return Json(new { success = false, message = error });
        }

        [HttpPost]
        public IActionResult UpdateCart(Dictionary<int, int> quantities)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            bool hasErrors = false;
            foreach (var kvp in quantities)
            {
                int productId = kvp.Key;
                int quantity = kvp.Value;

                // Get cart to find the cart item
                var (cartVM, _) = _cartService.GetUserCart(userId);
                var cartItem = cartVM.Items.FirstOrDefault(i => i.ProductId == productId);
                
                if (cartItem != null)
                {
                    var (success, error) = _cartService.UpdateCartItem(userId, cartItem.CartItemId, quantity);
                    if (!success)
                    {
                        hasErrors = true;
                        TempData["Error"] = error;
                    }
                }
            }

            if (!hasErrors)
            {
                TempData["Success"] = "Cart updated successfully";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCartItem(int cartItemId, int quantity)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            var (success, error) = _cartService.UpdateCartItem(userId, cartItemId, quantity);
            
            if (success)
            {
                return Json(new { success = true, message = "Cart updated" });
            }

            return Json(new { success = false, message = error });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            var (success, error) = _cartService.RemoveFromCart(userId, cartItemId);
            
            if (success)
            {
                var (count, _) = _cartService.GetCartItemCount(userId);
                return Json(new { success = true, cartCount = count, message = "Item removed from cart" });
            }

            return Json(new { success = false, message = error });
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Please login first" });
            }

            var (success, error) = _cartService.ClearCart(userId);
            
            if (success)
            {
                return Json(new { success = true, message = "Cart cleared" });
            }

            return Json(new { success = false, message = error });
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { count = 0 });
            }

            var (count, _) = _cartService.GetCartItemCount(userId);
            return Json(new { count = count });
        }
}