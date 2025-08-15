
using Cartify.DAL.DataBase;
using CartifyDAL.Entities.cart;
using CartifyDAL.Repo.cartRepo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.cartRepo.Implementation
{
    public class CartItemRepo : ICartItemRepo
    {
        private readonly CartifyDbContext _context;

        public CartItemRepo(CartifyDbContext context)
        {
            _context = context;
        }

        public (bool, string?) Create(CartItem cartItem)
        {
            try
            {
                _context.CartItem.Add(cartItem);
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<CartItem>, string?) GetAll()
        {
            try
            {
                var cartItems = _context.CartItem
                    .Where(ci => !ci.IsDeleted)
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Cart)
                    .ToList();
                return (cartItems, null);
            }
            catch (Exception ex)
            {
                return (new List<CartItem>(), ex.Message);
            }
        }

        public (List<CartItem>, string?) GetByCartId(int cartId)
        {
            try
            {
                var cartItems = _context.CartItem
                    .Where(ci => ci.CartId == cartId && !ci.IsDeleted)
                    .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                    .ToList();
                return (cartItems, null);
            }
            catch (Exception ex)
            {
                return (new List<CartItem>(), ex.Message);
            }
        }

        public (CartItem, string?) GetById(int cartItemId)
        {
            try
            {
                var cartItem = _context.CartItem
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Cart) // load Cart so UserId is available
                    .FirstOrDefault(ci => ci.Cartitem == cartItemId && !ci.IsDeleted);

                if (cartItem == null)
                    return (null, "Cart item not found");

                return (cartItem, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (CartItem, string?) GetByCartAndProduct(int cartId, int productId)
        {
            try
            {
                var cartItem = _context.CartItem
                    .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId && !ci.IsDeleted);
                
                return (cartItem, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (bool, string?) Update(CartItem cartItem)
        {
            try
            {
                _context.CartItem.Update(cartItem);
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        //public (bool, string?) Delete(int cartItemId)
        //{
        //    try
        //    {
        //        var cartItem = _context.CartItem.FirstOrDefault(ci => ci.Cartitem == cartItemId);
        //        if (cartItem == null)
        //            return (false, "Cart item not found");

        //        cartItem.Delete("System");
        //        _context.SaveChanges();
        //        return (true, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return (false, ex.Message);
        //    }
        //}

        public (bool, string?) Delete(int cartItemId)
        {
            try
            {
                var cartItem = _context.CartItem.FirstOrDefault(ci => ci.Cartitem == cartItemId);
                if (cartItem == null)
                    return (false, "Cart item not found");

                cartItem.Delete("System");
                _context.Entry(cartItem).State = EntityState.Modified;
                _context.SaveChanges();
                _context.Entry(cartItem).State = EntityState.Detached; // prevent stale cache

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


        public (bool, string?) DeleteByCartId(int cartId)
        {
            try
            {
                var cartItems = _context.CartItem.Where(ci => ci.CartId == cartId).ToList();
                _context.CartItem.RemoveRange(cartItems);
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
   }
}
