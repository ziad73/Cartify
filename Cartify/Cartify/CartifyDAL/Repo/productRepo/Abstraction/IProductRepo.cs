using CartifyDAL.Entities.category;
using CartifyDAL.Entities.product;

namespace CartifyDAL.Repo.productRepo.Abstraction
{
    public interface IProductRepo
    {
        (bool, string?) Create(Product product);
        (List<Product>, string?) GetAll();
        (Product, string?) GetById(int productId);
        (bool, string?) Update(Product product);
        (bool, string?) Delete(int productId);
    }
}
