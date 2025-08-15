using AutoMapper;
using CartifyBLL.Services.CategoryServices;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.Services.Product.Impelementation;
using CartifyBLL.ViewModels.Product;
using CartifyDAL.Entities.product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CartifyPLL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public ProductController(IProductService productService, ICategoryService categoryService, IMapper _mapper)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            mapper = _mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result =await productService.GetAll();
            return View(result.Item1);
        }
        public IActionResult AddNewProduct()
        {
            var (categories, msg) = categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            return View();
        }
        public async Task<IActionResult> AddProduct(CreateProduct model)
        {
            if (!ModelState.IsValid)
            {
                var (categories, msg) = categoryService.GetAll();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
                return View(model);
            }



            model.CreatedBy = User.Identity?.Name ?? "System"; 

            var result =await productService.Create(model);

            if (!result.Item1)
            {
                ModelState.AddModelError(string.Empty, result.Item2 ?? "Error occurred while adding the product!");
                return View(model);
            }

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await productService.GetById(id);
            if (product.Item1 == null)
                return NotFound();

            var (categories, msg) = categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

            var productDto = mapper.Map<CreateProduct>(product.Item1);
            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateProduct model)
        {
            if (ModelState.IsValid)
            {
                var (categories, msg) = categoryService.GetAll();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
                var result = await productService.Update(model);
                if (result.Item1)
                    return RedirectToAction("Index","Product"); 

                ModelState.AddModelError("", "Error updating product");
            }
            model.ModifiedBy = User.Identity?.Name ?? "System";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var (categories, msg) = categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            var product =await productService.GetById(id);
            if (product.Item1 == null)
                return NotFound();

            return View(product.Item1); 
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await productService.Delete(id);
            if (result.Item1)
                return RedirectToAction(nameof(Index));
            var (categories, msg) = categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            ModelState.AddModelError("", "Error deleting product");
            return View();
        }

    }
}
