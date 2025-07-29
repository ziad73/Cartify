using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.category;

namespace CartifyDAL.Repo.CartRepo.Abstraction
{
    public interface ICartItemRepo
    {
        (bool, string?) Create(CartItem cartItem);
        (List<CartItem>, string?) GetAll();
        (CartItem, string?) GetById(int cartItemId);
        (bool, string?) Update(CartItem cartItem);
        (bool, string?) Delete(int cartItemId);
    }
}
