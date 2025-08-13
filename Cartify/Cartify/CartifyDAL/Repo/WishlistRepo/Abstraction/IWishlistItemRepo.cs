using CartifyDAL.Entities.Wishlist;

namespace CartifyDAL.Repo.WishlistRepo.Abstraction;

public interface IWishlistItemRepo
{
    (bool, string?) Create(WishlistItem wishlistItem);
    (List<WishlistItem>, string?) GetAll();
    (List<WishlistItem>, string?) GetByWishlistId(int wishlistId);
    (WishlistItem, string?) GetById(int id);
    (WishlistItem, string?) GetByWishlistAndProduct(int wishlistId, int productId);
    (bool, string?) Delete(int id);
    (bool, string?) DeleteByWishlistId(int wishlistId);
    (int, string?) CountByWishlistId(int wishlistId);
}