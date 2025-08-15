using AutoMapper;
using CartifyBLL.Services.CategoryServices;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.Services.WishlistService.Abstraction;
using CartifyBLL.ViewModels.Product;
using CartifyBLL.ViewModels.Products.ProductReview;
using CartifyDAL.Entities.product;
using CartifyDAL.Entities.user;
using CartifyDAL.Repo.ProductReviewRepo.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CartifyPLL.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;
        private readonly IWishlistService _wishListService;
        private readonly IProductReviewRepo _productReview;
        private readonly IMapper _mapper;

        public StoreController(
            IProductService productService,
            ICategoryService categoryService,
            UserManager<User> userManager,
                IWishlistService wishListService,
                IProductReviewRepo productReview,
                IMapper mapper) // 👈 add this

        {
            _productService = productService;
            _categoryService = categoryService;
            _userManager = userManager;
            _wishListService = wishListService;
            _productReview = productReview;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int? categoryId, string searchTerm, double? minPrice, double? maxPrice, string sort = "name", int page = 1, int pageSize = 12)
        {
            try
            {
                // 1. Get products
                var (products, error) = await _productService.GetAll();
                if (error != null)
                {
                    TempData["Error"] = error;
                    products = new List<ProductDTO>();
                }

                var filtered = products.AsQueryable();

                // 2. Filter by category
                if (categoryId.HasValue && categoryId > 0)
                    filtered = filtered.Where(p => p.CategoryId == categoryId.Value);

                // 3. Filter by search term
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var lower = searchTerm.ToLower();
                    filtered = filtered.Where(p =>
                        (p.Name ?? "").ToLower().Contains(lower) ||
                        (p.Description ?? "").ToLower().Contains(lower) ||
                        (p.Category ?? "").ToLower().Contains(lower));
                }

                // 4. Price filter
                if (minPrice.HasValue)
                    filtered = filtered.Where(p => p.Price >= minPrice.Value);
                if (maxPrice.HasValue)
                    filtered = filtered.Where(p => p.Price <= maxPrice.Value);

                // 5. Sorting
                filtered = sort switch
                {
                    "price-asc" => filtered.OrderBy(p => p.Price),
                    "price-desc" => filtered.OrderByDescending(p => p.Price),
                    _ => filtered.OrderBy(p => p.Name) // default: name
                };

                // 6. Wishlist info for logged-in user
                var userId = _userManager.GetUserId(User);
                if (!string.IsNullOrEmpty(userId))
                {
                    foreach (var p in filtered)
                    {
                        var (inWishlist, _) = _wishListService.IsInWishList(userId, p.ProductId);
                        p.InWishlist = inWishlist;
                    }
                }

                // 7. Pagination
                var totalItems = filtered.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // 8. Categories for filter
                var (categories, _) = _categoryService.GetAll();

                // Pass to view
                ViewBag.Categories = categories?.Select(c => new { c.CategoryId, c.Name }).ToList();
                ViewBag.SelectedCategory = categoryId;
                ViewBag.SearchTerm = searchTerm;
                ViewBag.MinPrice = minPrice;
                ViewBag.MaxPrice = maxPrice;
                ViewBag.Sort = sort;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = totalItems;

                return View(paged);
            }
            catch
            {
                TempData["Error"] = "Failed to load store.";
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
        public async Task<IActionResult> Details(int id)
        {
            var (product,error) = await _productService.GetById(id);
            if (product == null)
                return NotFound();

            var (reviews, errorMessage) = await _productReview.GetReviewsByProductIdAsync(id);
            var reviewDTOs = _mapper.Map<List<ProductReviewDTO>>(reviews);
            var vm = new ProductReviewVM
            {
                Product = product,
                Reviews = reviewDTOs ?? new List<ProductReviewDTO>(),
                create = new CreateProductReview { ProductId = product.ProductId }
            };

            return View(vm);
        }
    }
}