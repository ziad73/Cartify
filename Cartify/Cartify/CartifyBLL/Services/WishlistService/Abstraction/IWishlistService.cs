using CartifyBLL.ViewModels.Wishlist;

namespace CartifyBLL.Services.WishlistService.Abstraction;

public interface IWishlistService
{
    (WishlistVm, string?) GetUserWishList(string userId);
    Task<(bool, string?)> AddToWishList(string userId, int productId);
    (bool, string?) RemoveFromWishList(string userId, int productId);
    (bool, string?) ClearWishList(string userId);
    Task<(bool, string?)> MoveToCart(string userId, int productId);
    (int, string?) GetWishListCount(string userId);
    (bool, string?) IsInWishList(string userId, int productId);
}