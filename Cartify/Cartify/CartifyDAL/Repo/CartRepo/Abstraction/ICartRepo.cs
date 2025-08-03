
using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.category;

namespace CartifyDAL.Repo.cartRepo.Abstraction
{
    public interface ICartRepo
    {
        (bool, string?) Create(Cart cart);
        (List<Cart>, string?) GetAll();
        (Cart, string?) GetById(int cartId);
        (bool, string?) Update(Cart cart);
        (bool, string?) Delete(int cartId);
    }
}
