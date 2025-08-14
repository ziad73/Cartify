using CartifyDAL.Entities.Wishlist;

namespace CartifyDAL.Repo.WishlistRepo.Abstraction;

public interface IWishlistRepo
{
    (bool, string?) Create(Wishlist wishlist);
    (Wishlist, string?) GetByUserId(string userId);     // single wishlist per user
    (bool, string?) Delete(int wishlistId);
}