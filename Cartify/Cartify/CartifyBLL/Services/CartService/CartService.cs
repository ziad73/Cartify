using Cartify.DAL.DataBase;
using CartifyBLL.ViewModels.Cart;
using CartifyDAL.Entities.productCart;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CartifyBLL.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly CartifyDbContext _context;
        private readonly ILogger<CartService> _logger;

        public CartService(CartifyDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartVm> GetUserCartAsync(string userId)
        {
            try
            {
                var cartItems = await _context.ProductCart
                    .Include(pc => pc.product)
                    .Where(pc => pc.cart.CreatedBy == userId && !pc.IsDeleted && !pc.cart.IsDeleted)
                    .ToListAsync();

                return new CartVm
                {
                    Items = cartItems.Select(item => new CartItemVm
                    {
                        ProductId = item.ProductId,
                        ProductName = item.product.Description, // Using Description as ProductName
                        Price = (decimal)item.product.Price
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user cart for user {UserId}", userId);
                return new CartVm();
            }
        }

        public async Task<bool> AddToCartAsync(string userId, int productId, int quantity = 1)
        {
            try
            {
                var cart = await _context.Cart
                    .FirstOrDefaultAsync(c => c.CreatedBy == userId && !c.IsDeleted);

                if (cart == null)
                {
                    cart = new CartifyDAL.Entities.cart.Cart(userId);
                    _context.Cart.Add(cart);
                    await _context.SaveChangesAsync();
                }

                var cartItem = await _context.ProductCart
                    .FirstOrDefaultAsync(pc => pc.CartId == cart.CartId && pc.ProductId == productId && !pc.IsDeleted);

                if (cartItem != null)
                {
                    cartItem.ModifiedBy = userId;
                    cartItem.ModifiedOn = DateTime.Now;
                }
                else
                {
                    cartItem = new ProductCart(userId)
                    {
                        CartId = cart.CartId,
                        ProductId = productId,
                        
                    };
                    _context.ProductCart.Add(cartItem);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            try
            {
                var cartItem = await _context.ProductCart
                    .Include(pc => pc.cart)
                    .FirstOrDefaultAsync(pc => pc.cart.CreatedBy == userId && pc.ProductId == productId && !pc.IsDeleted);

                if (cartItem == null)
                    return false;

                if (quantity <= 0)
                {
                    cartItem.IsDeleted = true;
                    cartItem.DeletedBy = userId;
                    cartItem.DeletedOn = DateTime.Now;
                }
                else
                {
                    cartItem.ModifiedBy = userId;
                    cartItem.ModifiedOn = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart quantity for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> RemoveFromCartAsync(string userId, int productId)
        {
            try
            {
                var cartItem = await _context.ProductCart
                    .Include(pc => pc.cart)
                    .FirstOrDefaultAsync(pc => pc.cart.CreatedBy == userId && pc.ProductId == productId && !pc.IsDeleted);

                if (cartItem == null)
                    return false;

                cartItem.IsDeleted = true;
                cartItem.DeletedBy = userId;
                cartItem.DeletedOn = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            try
            {
                var cart = await _context.Cart
                    .FirstOrDefaultAsync(c => c.CreatedBy == userId && !c.IsDeleted);

                if (cart == null)
                    return true;

                var cartItems = await _context.ProductCart
                    .Where(pc => pc.CartId == cart.CartId && !pc.IsDeleted)
                    .ToListAsync();

                foreach (var item in cartItems)
                {
                    item.IsDeleted = true;
                    item.DeletedBy = userId;
                    item.DeletedOn = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
                return false;
            }
        }
    }
}