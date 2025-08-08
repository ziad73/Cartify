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

    public async Task<Wishlist> GetWishlistByUserIdAsync(string userId)
    {
        return await _context.Wishlists
            .Include(w => w.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task AddToWishlistAsync(WishlistItem item)
    {
        await _context.WishlistItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromWishlistAsync(int itemId)
    {
        var item = await _context.WishlistItems.FindAsync(itemId);
        if (item != null)
        {
            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<WishlistItem>> GetWishlistItemsAsync(int wishlistId)
    {
        return await _context.WishlistItems
            .Where(i => i.WishlistId == wishlistId)
            .Include(i => i.Product)
            .ToListAsync();
    }
}