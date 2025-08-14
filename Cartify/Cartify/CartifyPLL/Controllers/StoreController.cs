using AutoMapper;
using CartifyBLL.Services.CategoryServices;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.Services.Product.Impelementation;
using CartifyBLL.ViewModels.Product;
using CartifyBLL.ViewModels.Products.ProductReview;
using CartifyDAL.Repo.ProductReviewRepo.Abstraction;
using CartifyPLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CartifyPLL.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductReviewRepo _productReview;
        private readonly IMapper _mapper;

        public StoreController(IProductService productService, ICategoryService categoryService, IProductReviewRepo productReview, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _productReview = productReview;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var (products, productError) = await _productService.GetAll();
            var (categories, categoryError) = _categoryService.GetAll();

            var viewModel = new StoreViewModel
            {
                Products = products ?? new List<CartifyBLL.ViewModels.Product.ProductDTO>(),
                Categories = categories ?? new List<CartifyDAL.Entities.category.Category>()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetById(id);
            if (product.Item1 == null)
                return NotFound();

            var (reviews, errorMessage) = await _productReview.GetReviewsByProductIdAsync(id);
            var reviewDTOs = _mapper.Map<List<ProductReviewDTO>>(reviews);
            var vm = new ProductReviewVM
            {
                Product = product.Item1,
                Reviews = reviewDTOs ?? new List<ProductReviewDTO>(),
                create = new CreateProductReview { ProductId = product.Item1.ProductId }
            };

            return View(vm);
        }
    }
}
