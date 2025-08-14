
using CartifyDAL.Entities.productCart;

namespace CartifyDAL.Repo.ProductCartRepo.Abstraction
{
    public interface IProductCartRepo
    {
        (bool, string?) Create(ProductCart productCart);
        (List<ProductCart>, string?) GetAll();
        (ProductCart, string?) GetById(int productCartId);
        (bool, string?) Update(ProductCart productCart);
        (bool, string?) Delete(int productCartId);
    }
}
