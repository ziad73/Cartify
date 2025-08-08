using Cartify.DAL.DataBase;
using CartifyBLL.Services.Wishlist.Abstraction;
using CartifyDAL.Entities.Wishlist;
using Microsoft.EntityFrameworkCore;

namespace CartifyBLL.Services.Wishlist.Implementation;

public class WishlistService : IWishlisrService
{
    private readonly CartifyDbContext _context;

        public WishlistService(CartifyDbContext context)
        {
            _context = context;
        }

        public async Task<CartifyDAL.Entities.Wishlist.Wishlist> GetWishlistByUserIdAsync(string userId)
        {
            return await _context.Wishlists
                .Include(w => w.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task AddToWishlistAsync(string userId, int productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new CartifyDAL.Entities.Wishlist.Wishlist { UserId = userId };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync(); // Save to get WishlistId
            }

            var exists = wishlist.Items.Any(i => i.ProductId == productId);
            if (!exists)
            {
                wishlist.Items.Add(new WishlistItem
                {
                    ProductId = productId,
                    WishlistId = wishlist.Id
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromWishlistAsync(string userId, int productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist != null)
            {
                var item = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    wishlist.Items.Remove(item);
                    _context.WishlistItems.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task ClearWishlistAsync(string userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist != null)
            {
                _context.WishlistItems.RemoveRange(wishlist.Items);
                wishlist.Items.Clear();
                await _context.SaveChangesAsync();
            }
        }
    }
