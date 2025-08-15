using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.category;

namespace CartifyDAL.Repo.cartRepo.Abstraction
{
    public interface ICartRepo
    {
        (bool, string?) Create(Cart cart);
        (List<Cart>, string?) GetAll();
        (CartItem, string?) GetById(int cartId);
        (Cart, string?) GetByUserId(string userId);
        (bool, string?) Update(Cart cart);
        (bool, string?) Delete(int cartId);
        (bool, string?) ClearCart(string userId);
    }
}
