
using Cartify.DAL.DataBase;
using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.category;
using CartifyDAL.Repo.cartRepo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.cartRepo.Implementation
{
   public class CartRepo : ICartRepo
   {

       private readonly CartifyDbContext _context;

        public CartRepo(CartifyDbContext context)
        {
            _context = context;
        }

        public (bool, string?) Create(Cart cart)
        {
            try
            {
                _context.Cart.Add(cart);
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<Cart>, string?) GetAll()
        {
            try
            {
                var carts = _context.Cart
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.cartItems)
                    .ThenInclude(ci => ci.Product)
                    .ToList();
                return (carts, null);
            }
            catch (Exception ex)
            {
                return (new List<Cart>(), ex.Message);
            }
        }

        public (CartItem, string?) GetById(int cartItemId)
        {
            try
            {
                var cartItem = _context.CartItem
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Cart)        // ✅ add this
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

        public (Cart, string?) GetByUserId(string userId)
        {
            try
            {
                var cart = _context.Cart
                    .Include(c => c.cartItems
                        .Where(ci => !ci.IsDeleted)) // filter here
                    .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                    .FirstOrDefault(c => c.UserId == userId);

                return (cart, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (bool, string?) Update(Cart cart)
        {
            try
            {
                _context.Cart.Update(cart);
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) Delete(int cartId)
        {
            try
            {
                var cart = _context.Cart.FirstOrDefault(c => c.CartId == cartId);
                if (cart == null)
                    return (false, "Cart not found");

                cart.Delete("System");
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) ClearCart(string userId)
        {
            try
            {
                var cart = _context.Cart
                    .Include(c => c.cartItems)
                    .FirstOrDefault(c => c.UserId == userId && !c.IsDeleted);

                if (cart != null && cart.cartItems != null)
                {
                    _context.CartItem.RemoveRange(cart.cartItems);
                    _context.SaveChanges();
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
   }
}
