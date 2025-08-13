using CartifyBLL.ViewModels.Cart;

namespace CartifyBLL.Services.CartService.Abstraction;

public interface ICartService
{
    (CartVm, string?) GetUserCart(string userId);
    (bool, string?) AddToCart(string userId, AddToCartVm model);
    (bool, string?) UpdateCartItem(string userId, int cartItemId, int quantity);
    (bool, string?) RemoveFromCart(string userId, int cartItemId);
    (bool, string?) ClearCart(string userId);
    (int, string?) GetCartItemCount(string userId);
}