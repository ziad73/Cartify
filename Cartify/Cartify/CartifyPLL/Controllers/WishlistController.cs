using System.Security.Claims;
using CartifyBLL.Services.Wishlist.Abstraction;
using CartifyDAL.Entities.Wishlist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers;

public class WishlistController : Controller
{
   
        private readonly IWishlisrService _wishlistService;

        public WishlistController(IWishlisrService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // Optionally redirect to login or show an error page
                return RedirectToAction("Login", "Account");
            }

            var wishlist = await _wishlistService.GetWishlistByUserIdAsync(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    Items = new List<WishlistItem>() // or your exact type
                };
            }

            return View(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var userId = GetUserId();
            await _wishlistService.AddToWishlistAsync(userId, productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetUserId();
            await _wishlistService.RemoveFromWishlistAsync(userId, productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = GetUserId();
            await _wishlistService.ClearWishlistAsync(userId);
            return RedirectToAction("Index");
        }
    }
