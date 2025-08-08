namespace CartifyBLL.Services.Wishlist.Abstraction;

public interface IWishlisrService
{
    Task<CartifyDAL.Entities.Wishlist.Wishlist> GetWishlistByUserIdAsync(string userId);
    Task AddToWishlistAsync(string userId, int productId);
    Task RemoveFromWishlistAsync(string userId, int productId);
    Task ClearWishlistAsync(string userId);
}