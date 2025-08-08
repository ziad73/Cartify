using CartifyBLL.ViewModels.Cart;

namespace CartifyBLL.Services.CartService
{
    public interface ICartService
    {
        Task<CartVm> GetUserCartAsync(string userId);
        Task<bool> AddToCartAsync(string userId, int productId, int quantity = 1);
        Task<bool> UpdateQuantityAsync(string userId, int productId, int quantity);
        Task<bool> RemoveFromCartAsync(string userId, int productId);
        Task<bool> ClearCartAsync(string userId);
    }
}