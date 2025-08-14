using CartifyBLL.Services.CategoryServices;
using CartifyDAL.Entities.category;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // retieve all categories and display them in the index view
        public IActionResult Index()
        {
            var (categories, error) = _categoryService.GetAll();
            if (error != null) ViewBag.Error = error;
            return View(categories);
        }
        // show the create view to add a new category
        public IActionResult Create() => View();

        // handle the creation of a new category
        [HttpPost]
        public IActionResult Create(string name, string? description)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "Name is required.";
                return View();
            }

            var category = new Category(name, description, "admin"); // Replace with actual user if needed
            var (success, error) = _categoryService.Create(category);
            if (!success) ViewBag.Error = error;

            return RedirectToAction("Index");
        }
        // show form fiiled with data 
        public IActionResult Edit(int id)
        {
            var (category, error) = _categoryService.GetById(id);
            if (error != null) return NotFound();
            return View(category);
        }
        // save the edited category
        [HttpPost]
        public IActionResult Edit(int id, string name, string? description)
        {
            var (existingCategory, error) = _categoryService.GetById(id);
            if (existingCategory == null) return NotFound();

            existingCategory.Update(name, description, null, "admin"); // Replace with actual user
            var (success, updateError) = _categoryService.Update(existingCategory);
            if (!success) ViewBag.Error = updateError;

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);
            return RedirectToAction("Index"); // Or whatever your listing action is
        }
    }

}