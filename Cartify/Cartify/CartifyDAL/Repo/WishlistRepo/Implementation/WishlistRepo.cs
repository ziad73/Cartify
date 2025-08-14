using Cartify.DAL.DataBase;
using CartifyDAL.Entities.Wishlist;
using CartifyDAL.Repo.WishlistRepo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.WishlistRepo.Implementation;

public class WishlistRepo : IWishlistRepo
{
    private readonly CartifyDbContext _context;

    public WishlistRepo(CartifyDbContext context)
    {
        _context = context;
    }

    public (bool, string?) Create(Wishlist wishlist)
    {
        try
        {
            _context.Wishlists.Add(wishlist);
            _context.SaveChanges();
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // Single wishlist per user
    public (Wishlist, string?) GetByUserId(string userId)
    {
        try
        {
            var wishlist = _context.Wishlists
                .Include(w => w.Items)
                .ThenInclude(wi => wi.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefault(w => w.UserId == userId && !w.IsDeleted);

            return (wishlist, null);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    public (bool, string?) Delete(int wishlistId)
    {
        try
        {
            var wishlist = _context.Wishlists.FirstOrDefault(w => w.WishListId == wishlistId);
            if (wishlist == null) return (false, "Wishlist not found");

            wishlist.Delete("System");
            _context.SaveChanges();
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}