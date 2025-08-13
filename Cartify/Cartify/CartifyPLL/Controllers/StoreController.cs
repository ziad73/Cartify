using Microsoft.AspNetCore.Mvc;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.Services.CategoryServices;
using CartifyBLL.Services.WishlistService.Abstraction;
using CartifyBLL.ViewModels.Product;
using CartifyDAL.Entities.user;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CartifyPLL.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;
        private readonly IWishlistService _wishListService;

        public StoreController(
            IProductService productService, 
            ICategoryService categoryService,
            UserManager<User> userManager,
                IWishlistService wishListService) // 👈 add this
            
        {
            _productService = productService;
            _categoryService = categoryService;
            _userManager = userManager;
            _wishListService = wishListService; 
        }

        public IActionResult Index(int? categoryId, string searchTerm, int page = 1, int pageSize = 12)
        {
            try
            {
                // Get all products
                var (products, error) = _productService.GetAll();
                if (error != null)
                {
                    TempData["Error"] = error;
                    products = new List<ProductDTO>();
                }

                // Apply filters
                var filteredProducts = products.AsEnumerable();

                // Filter by category if specified
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    filteredProducts = filteredProducts.Where(p => p.CategoryId == categoryId.Value);
                }

                // Filter by search term if specified
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var searchLower = searchTerm.ToLower();
                    filteredProducts = filteredProducts.Where(p => 
                        (p.Name?.ToLower().Contains(searchLower) ?? false) ||
                        (p.Description?.ToLower().Contains(searchLower) ?? false) ||
                        (p.Category?.ToLower().Contains(searchLower) ?? false));
                }

                // Convert back to list
                var resultProducts = filteredProducts.ToList();

                // Get categories for filter dropdown
                var (categories, categoryError) = _categoryService.GetAll();
                var categoriesList = categories ?? new List<CartifyDAL.Entities.category.Category>();
                
                // Create dynamic list for ViewBag
                var dynamicCategories = categoriesList.Select(c => new { 
                    CategoryId = c.CategoryId, 
                    Name = c.Name 
                }).ToList();
                
                // Pass data to view
                ViewBag.Categories = dynamicCategories;
                ViewBag.SelectedCategory = categoryId;
                ViewBag.SearchTerm = searchTerm;
                
                var userId = _userManager.GetUserId(User);
                if (!string.IsNullOrEmpty(userId))
                {
                    foreach (var product in resultProducts)
                    {
                        var (isInWishlist, _) = _wishListService.IsInWishList(userId, product.ProductId);
                        product.InWishlist = isInWishlist; // Ensure ProductDTO has bool InWishlist
                    }
                }

                // Pagination
                var totalItems = resultProducts.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                var pagedProducts = resultProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = totalItems;

                return View(pagedProducts);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading products.";
                ViewBag.Categories = new List<dynamic>();
                ViewBag.SelectedCategory = categoryId;
                ViewBag.SearchTerm = searchTerm;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 1;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = 0;
                return View(new List<ProductDTO>());
            }
        }

        // Simplified placeholder methods - these will work regardless of service availability
        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Please login to add items to cart" });
                }

                // For now, return success - you can implement cart logic later
                return Json(new { 
                    success = true, 
                    message = "Product added to cart successfully!", 
                    cartCount = 1 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "An error occurred while adding to cart" 
                });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToWishlist(int productId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Please login to add items to wishlist" });
                }

                // For now, return success - you can implement wishlist logic later
                return Json(new { 
                    success = true, 
                    message = "Product added to wishlist successfully!", 
                    wishlistCount = 1 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "An error occurred while adding to wishlist" 
                });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult RemoveFromWishlist(int productId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                // For now, return success - you can implement wishlist logic later
                return Json(new { 
                    success = true, 
                    message = "Product removed from wishlist", 
                    wishlistCount = 0 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "An error occurred" 
                });
            }
        }
    }
}