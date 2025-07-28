using CartifyDAL.Entities.category;

namespace CartifyDAL.Repo.categoryRepo.Abstraction
{
    public interface ICategoryRepo
    {

        (bool, string?) Create(Category category);
        (List<Category>,string?) GetAll();
        (Category,string?) GetById(int categoryId);
        (bool,string?) Update(Category category);
        (bool, string?) Delete(int categoryId);
    }
}
