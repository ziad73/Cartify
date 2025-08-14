using Cartify.DAL.DataBase;
using CartifyDAL.Entities.Wishlist;
using CartifyDAL.Repo.WishlistRepo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.WishlistRepo.Implementation;

public class WishlistItemRepo : IWishlistItemRepo
{
     private readonly CartifyDbContext _context;

        public WishlistItemRepo(CartifyDbContext context)
        {
            _context = context;
        }

        public (bool, string?) Create(WishlistItem wishlistItem)
        {
            try
            {
                _context.WishlistItems.Add(wishlistItem);
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<WishlistItem>, string?) GetAll()
        {
            try
            {
                var items = _context.WishlistItems
                    .Where(wi => !wi.IsDeleted)
                    .Include(wi => wi.Product)
                    .Include(wi => wi.Wishlist)
                    .ToList();
                return (items, null);
            }
            catch (Exception ex)
            {
                return (new List<WishlistItem>(), ex.Message);
            }
        }

        public (List<WishlistItem>, string?) GetByWishlistId(int wishlistId)
        {
            try
            {
                var items = _context.WishlistItems
                    .Where(wi => wi.WishlistId == wishlistId && !wi.IsDeleted)
                    .Include(wi => wi.Product)
                    .ToList();
                return (items, null);
            }
            catch (Exception ex)
            {
                return (new List<WishlistItem>(), ex.Message);
            }
        }

        public (WishlistItem, string?) GetById(int id)
        {
            try
            {
                var item = _context.WishlistItems
                    .Include(wi => wi.Wishlist)
                    .FirstOrDefault(wi => wi.Id == id && !wi.IsDeleted);

                if (item == null) return (null, "Wishlist item not found");
                return (item, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (WishlistItem, string?) GetByWishlistAndProduct(int wishlistId, int productId)
        {
            try
            {
                var item = _context.WishlistItems
                    .FirstOrDefault(wi => wi.WishlistId == wishlistId && wi.ProductId == productId && !wi.IsDeleted);
                return (item, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (bool, string?) Delete(int id)
        {
            try
            {
                var item = _context.WishlistItems.FirstOrDefault(wi => wi.Id == id && !wi.IsDeleted);
                if (item == null) return (false, "Wishlist item not found");

                item.Delete("System");
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) DeleteByWishlistId(int wishlistId)
        {
            try
            {
                var items = _context.WishlistItems.Where(wi => wi.WishlistId == wishlistId && !wi.IsDeleted).ToList();
                foreach (var item in items) item.Delete("System");
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (int, string?) CountByWishlistId(int wishlistId)
        {
            try
            {
                var count = _context.WishlistItems.Count(wi => wi.WishlistId == wishlistId && !wi.IsDeleted);
                return (count, null);
            }
            catch (Exception ex)
            {
                return (0, ex.Message);
            }
        }
}