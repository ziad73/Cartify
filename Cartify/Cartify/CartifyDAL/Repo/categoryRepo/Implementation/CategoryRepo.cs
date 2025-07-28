

using Cartify.DAL.DataBase;
using CartifyDAL.Entities.category;
using CartifyDAL.Entities.order;
using CartifyDAL.Repo.categoryRepo.Abstraction;

namespace CartifyDAL.Repo.CategoryRepo.Implementation
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly CartifyDbContext db;

        public CategoryRepo(CartifyDbContext db)
        {
            this.db = db;
        }
        public (bool, string?) Create(Category category)
        {
            try
            {
                db.Category.Add(category);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) Delete(int categoryId)
        {
            try
            {
                var category = db.Category.FirstOrDefault(a => a.CategoryId == categoryId);
                if (category == null)
                {
                    return (false, "Category not found");
                }
                category.Delete(category.DeletedBy);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<Category>, string?) GetAll()
        {
            try
            {
                var categories = db.Category
                    .Where(a => !a.IsDeleted)
                    .ToList();
                return (categories, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (Category, string?) GetById(int categoryId)
        {
            try
            {
                var category = db.Category
                    .FirstOrDefault(a => a.CategoryId == categoryId && !a.IsDeleted);
                if (category == null)
                {
                    return (null, "Category not found");
                }
                return (category, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (bool, string?) Update(Category category)
        {
            try
            {
                var existingCategory = db.Category.FirstOrDefault(a => a.CategoryId ==category.CategoryId && !a.IsDeleted);
                if (existingCategory == null)
                {
                    return (false, "Category not found");
                }
                existingCategory.Update(category.Name, category.Description, category.ParentCategoryId, category.ModifiedBy);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
