using CartifyDAL.Entities.category;

namespace CartifyBLL.Services.CategoryServices
{

    public interface ICategoryService
    {
        (bool, string?) Create(Category category);
        (List<Category>, string?) GetAll();
        (Category, string?) GetById(int categoryId);
        (bool, string?) Update(Category category);
        (bool, string?) Delete(int categoryId);
    }
}