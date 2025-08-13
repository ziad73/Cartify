using System.Security.Claims;
using CartifyBLL.Helper;
using CartifyBLL.Services.CartService.Abstraction;
using CartifyBLL.Services.WishlistService.Abstraction;
using CartifyBLL.ViewModels.Wishlist;
using CartifyDAL.Entities.Wishlist;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers;

public class WishlistController : Controller
{
    private readonly IWishlistService _wishListService;
        private readonly ICartService _cartService;

        public WishlistController(IWishlistService wishListService, ICartService cartService)
        {
            _wishListService = wishListService;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var (wishListVM, error) = _wishListService.GetUserWishList(userId);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View(new CartifyBLL.ViewModels.Wishlist.WishlistVm());
            }

            return View(wishListVM);
        }

        [HttpPost]
        public IActionResult AddToWishList(int productId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please login first" });

            var (success, error) = _wishListService.AddToWishList(userId, productId);

            if (success)
            {
                var (count, _) = _wishListService.GetWishListCount(userId);
                return Json(new { success = true, wishlistCount = count, message = "Added to wishlist" });
            }

            return Json(new { success = false, message = error });
        }

        [HttpPost]
        public IActionResult RemoveFromWishList(int productId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please login first" });

            var (success, error) = _wishListService.RemoveFromWishList(userId, productId);

            if (success)
            {
                var (count, _) = _wishListService.GetWishListCount(userId);
                return Json(new { success = true, wishlistCount = count, message = "Item removed from wishlist" });
            }

            return Json(new { success = false, message = error });
        }

        [HttpPost]
        public IActionResult ClearWishList()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please login first" });

            var (success, error) = _wishListService.ClearWishList(userId);
            return Json(new { success, message = success ? "Wishlist cleared" : error });
        }

        [HttpPost]
        public IActionResult MoveToCart(int productId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please login first" });

            var (success, error) = _wishListService.MoveToCart(userId, productId);

            if (success)
            {
                var (cartCount, _) = _cartService.GetCartItemCount(userId);
                var (wishlistCount, _) = _wishListService.GetWishListCount(userId);

                return Json(new
                {
                    success = true,
                    cartCount,
                    wishlistCount,
                    message = "Item moved to cart"
                });
            }

            return Json(new { success = false, message = error });
        }

        [HttpGet]
        public IActionResult GetWishListCount()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Json(new { count = 0 });

            var (count, _) = _wishListService.GetWishListCount(userId);
            return Json(new { count });
        }

        [HttpGet]
        public IActionResult IsInWishList(int productId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Json(new { inWishlist = false });

            var (isInWishlist, _) = _wishListService.IsInWishList(userId, productId);
            return Json(new { inWishlist = isInWishlist });
        }
        
        [HttpPost]
        public IActionResult ToggleWishlist(int productId)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "Please login first" });

            // Check if it's already in wishlist
            var (isInWishlist, _) = _wishListService.IsInWishList(userId, productId);

            if (isInWishlist)
            {
                var (removed, error) = _wishListService.RemoveFromWishList(userId, productId);
                if (removed)
                {
                    var (count, _) = _wishListService.GetWishListCount(userId);
                    return Json(new { success = true, wishlistCount = count, message = "Removed from wishlist" });
                }
                return Json(new { success = false, message = error });
            }
            else
            {
                var (added, error) = _wishListService.AddToWishList(userId, productId);
                if (added)
                {
                    var (count, _) = _wishListService.GetWishListCount(userId);
                    return Json(new { success = true, wishlistCount = count, message = "Added to wishlist" });
                }
                return Json(new { success = false, message = error });
            }
        }
}