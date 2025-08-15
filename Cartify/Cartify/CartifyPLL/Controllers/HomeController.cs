using CartifyBLL.Services.CategoryServices;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.Services.Product.Impelementation;
using CartifyBLL.ViewModels.Home;
using CartifyBLL.ViewModels.Product;
using CartifyDAL.Entities.category;
using CartifyPLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CartifyPLL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        public HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService)
        {
            _logger = logger;
            this.productService = productService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var (products, error) = await productService.GetAll();

            var (categoryProducts, msg3) = await productService.GetAll();
            if (categoryId.HasValue)
                categoryProducts = categoryProducts.Where(p => p.CategoryId == categoryId.Value).ToList();

            var (categories, msg4) = categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            ViewBag.SelectedCategoryId = categoryId;

            if (!string.IsNullOrEmpty(error))
            {

                return View(new List<ProductDTO>());
            }


            var model = new HomeVM
            {
                NewProducts = products,
                CategoryProducts = categoryProducts
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Policy()
        {
            return View("Terms");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact()
        {
            return View("Contact");
        }
        public IActionResult Categories()
        {
            var categories = categoryService.GetAll();
            return Json(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryProducts(int? categoryId)
        {
            var (products, msg) = await productService.GetAll();

            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();

            return PartialView("_CategoryProducts", products);
        }



    }
}
