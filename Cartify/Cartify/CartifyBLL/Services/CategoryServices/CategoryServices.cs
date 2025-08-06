using CartifyDAL.Entities.category;
using CartifyDAL.Repo.categoryRepo.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace CartifyBLL.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryService(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }


        public (bool, string?) Create(Category category)
        {
            if (category == null)
                return (false, "Category data is missing.");

            if (string.IsNullOrWhiteSpace(category.Name))
                return (false, "Category name is required.");

            var (existingCategories, error) = _categoryRepo.GetAll();

            var duplicate = existingCategories?
                .FirstOrDefault(c => !c.IsDeleted &&
                                     !string.IsNullOrEmpty(c.Name) &&
                                     c.Name.ToLower() == category.Name.ToLower());

            if (duplicate != null)
                return (false, "A category with this name already exists.");

            return _categoryRepo.Create(category);
        }

        public (List<Category>, string?) GetAll()
        {
            var (categories, error) = _categoryRepo.GetAll();

            if (!string.IsNullOrEmpty(error))
                return (new List<Category>(), error);

            if (categories == null)
                return (new List<Category>(), "No categories found.");

            var activeCategories = categories.Where(c => !c.IsDeleted).ToList();
            return (activeCategories, null);

        }

        public (Category, string?) GetById(int categoryId)
        {
            var (category, error) = _categoryRepo.GetById(categoryId);
            if (category == null || category.IsDeleted)
                return (null, "Category not found.");

            return (category, null);
        }

        public (bool, string?) Update(Category updatedCategory)
        {
            if (updatedCategory == null || updatedCategory.CategoryId == 0)
                return (false, "Invalid category data.");

            if (string.IsNullOrWhiteSpace(updatedCategory.Name))
                return (false, "Category name is required.");

            var (existing, _) = _categoryRepo.GetById(updatedCategory.CategoryId);
            if (existing == null || existing.IsDeleted)
                return (false, "Category not found.");

            var (allCategories, _) = _categoryRepo.GetAll();
            var duplicate = allCategories
                .FirstOrDefault(c => c.Name.ToLower() == updatedCategory.Name.ToLower()
                                  && c.CategoryId != updatedCategory.CategoryId
                                  && !c.IsDeleted);

            if (duplicate != null)
                return (false, "Another category with the same name exists.");

            existing.Update(
                updatedCategory.Name,
                updatedCategory.Description,
                updatedCategory.ParentCategoryId,
                updatedCategory.ModifiedBy ?? "System"
            );

            return _categoryRepo.Update(existing);
        }

        public (bool, string?) Delete(int categoryId)
        {
            var (category, _) = _categoryRepo.GetById(categoryId);
            if (category == null || category.IsDeleted)
                return (false, "Category not found.");

            category.Delete("System");

            return _categoryRepo.Delete(categoryId);
        }
    }
}