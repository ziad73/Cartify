using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.category;

namespace CartifyDAL.Repo.cartRepo.Abstraction
{
    public interface ICartItemRepo
    {
        (bool, string?) Create(CartItem cartItem);
        (List<CartItem>, string?) GetAll();
        (List<CartItem>, string?) GetByCartId(int cartId);
        (CartItem, string?) GetById(int cartItemId);
        (CartItem, string?) GetByCartAndProduct(int cartId, int productId);
        (bool, string?) Update(CartItem cartItem);
        (bool, string?) Delete(int cartItemId);
        (bool, string?) DeleteByCartId(int cartId);
    }
}
