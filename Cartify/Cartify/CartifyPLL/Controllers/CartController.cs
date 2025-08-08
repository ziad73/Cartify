using CartifyBLL.Services.CartService;
using CartifyBLL.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CartifyWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User not logged in");
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetUserCartAsync(userId);
            return View(cart);
        }

        // POST: /Cart/Add
        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = GetUserId();
            var result = await _cartService.AddToCartAsync(userId, productId, quantity);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Failed to add item to cart");
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var userId = GetUserId();
            var result = await _cartService.UpdateQuantityAsync(userId, productId, quantity);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Failed to update quantity");
        }

        // POST: /Cart/Remove
        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetUserId();
            var result = await _cartService.RemoveFromCartAsync(userId, productId);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Failed to remove item from cart");
        }

        // POST: /Cart/Clear
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = GetUserId();
            var result = await _cartService.ClearCartAsync(userId);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Failed to clear cart");
        }
    }
}