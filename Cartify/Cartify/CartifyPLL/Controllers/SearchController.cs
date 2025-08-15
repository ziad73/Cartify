using CartifyBLL.Services.CategoryServices;
using CartifyBLL.Services.SearchService.Abstraction;
using CartifyBLL.Services.WishlistService.Abstraction;
using CartifyBLL.ViewModels.Product;
using CartifyBLL.ViewModels.Search;
using CartifyDAL.Entities.user;
using CartifyPLL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers;

public class SearchController : Controller
{

    private readonly ISearchService _searchService;
    private readonly ICategoryService _categoryService;
    private readonly UserManager<User> _userManager;
    private readonly IWishlistService _wishListService;

    public SearchController(
        ISearchService searchService,
        ICategoryService categoryService,
        UserManager<User> userManager,
        IWishlistService wishListService)
    {
        _searchService = searchService;
        _categoryService = categoryService;
        _userManager = userManager;
        _wishListService = wishListService;
    }

    [HttpGet]
    public IActionResult Index(string query, int page = 1, int pageSize = 12)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return RedirectToAction("Index", "Store");
        }

        // البحث عن المنتجات
        var searchResponse = _searchService.Search(query);
        var searchResults = searchResponse?.Results ?? new List<SearchResultDTO>();

        // تحويل SearchResultDTO إلى ProductDTO
        var products = searchResults.Select(r => new ProductDTO
        {
            ProductId = r.Id,
            Name = r.Name,
            Price = r.Price,
            ImageUrl = r.ImageUrl,
            Category = r.Category,
            CategoryId = r.CategoryId,
            StockQuantity = r.StockQuantity
        }).ToList();

        // جلب التصنيفات
        var (categories, categoryError) = _categoryService.GetAll();
        var categoriesList = categories ?? new List<CartifyDAL.Entities.category.Category>();

        var dynamicCategories = categoriesList.Select(c => new
        {
            CategoryId = c.CategoryId,
            Name = c.Name
        }).ToList();

        ViewBag.Categories = dynamicCategories;
        ViewBag.SelectedCategory = null;
        ViewBag.SearchTerm = query;

        // التحقق من المنتجات الموجودة في Wishlist
        var userId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(userId))
        {
            foreach (var product in products)
            {
                var (isInWishlist, _) = _wishListService.IsInWishList(userId, product.ProductId);
                product.InWishlist = isInWishlist;
            }
        }

        // Pagination
        var totalItems = products.Count;
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalItems = totalItems;

        // عرض نفس فيو الـ Store
        return View("~/Views/Store/Index.cshtml", pagedProducts);
    }
}