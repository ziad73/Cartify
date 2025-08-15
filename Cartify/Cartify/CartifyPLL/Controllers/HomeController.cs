using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.ViewModels.Product;
using CartifyPLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CartifyPLL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var (products, error) = await productService.GetAll();

            if (!string.IsNullOrEmpty(error))
            {
                
                return View(new List<ProductDTO>());
            }

            return View(products);
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
    }
}
