using Microsoft.AspNetCore.Mvc;

using CartifyPLL.Models;
using CartifyBLL.Services.CategoryServices;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.ViewModels.Product;

namespace CartifyPLL.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public StoreController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var (products, productError) = _productService.GetAll();
            var (categories, categoryError) = _categoryService.GetAll();

            var viewModel = new StoreViewModel
            {
                Products = products ?? new List<CartifyBLL.ViewModels.Product.ProductDTO>(),
                Categories = categories ?? new List<CartifyDAL.Entities.category.Category>()
            };

            return View(viewModel);
        }
    }
}
