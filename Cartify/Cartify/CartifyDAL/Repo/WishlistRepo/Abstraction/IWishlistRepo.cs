using CartifyDAL.Entities.Wishlist;

namespace CartifyDAL.Repo.WishlistRepo.Abstraction;

public interface IWishlistRepo
{
    Task<Wishlist> GetWishlistByUserIdAsync(string userId);
    Task AddToWishlistAsync(WishlistItem item);
    Task RemoveFromWishlistAsync(int itemId);
    Task<List<WishlistItem>> GetWishlistItemsAsync(int wishlistId);
}