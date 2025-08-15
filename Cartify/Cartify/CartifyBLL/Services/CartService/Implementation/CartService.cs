using AutoMapper;
using Cartify.DAL.DataBase;
using CartifyBLL.Services.CartService.Abstraction;
using CartifyBLL.ViewModels.Cart;
using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.productCart;
using CartifyDAL.Repo.cartRepo.Abstraction;
using CartifyDAL.Repo.productRepo.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CartifyBLL.Services.CartService.Implementation;

public class CartService : ICartService
{
    private readonly ICartRepo _cartRepo;
    private readonly ICartItemRepo _cartItemRepo;
    private readonly IProductRepo _productRepo;
    private readonly IMapper _mapper;

    public CartService(ICartRepo cartRepo, ICartItemRepo cartItemRepo, IProductRepo productRepo, IMapper mapper)
    {
        _cartRepo = cartRepo;
        _cartItemRepo = cartItemRepo;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public (CartVm, string?) GetUserCart(string userId)
    {
        try
        {
            var (cart, error) = _cartRepo.GetByUserId(userId);
            if (!string.IsNullOrEmpty(error))
                return (new CartVm { UserId = userId }, error);

            if (cart == null)
            {
                // Create new cart for user
                var newCart = new Cart(userId, userId);
                var (created, createError) = _cartRepo.Create(newCart);
                if (!created)
                    return (new CartVm { UserId = userId }, createError);

                return (new CartVm { CartId = newCart.CartId, UserId = userId }, null);
            }

            var cartVM = new CartVm
            {
                CartId = cart.CartId,
                UserId = userId,
                Items = cart.cartItems?.Select(ci => new CartItemVm
                {
                    CartItemId = ci.Cartitem,
                    ProductId = ci.Product.ProductId,
                    ProductName = ci.Product.Name,
                    ProductImageUrl = ci.Product.ImageUrl?
        .Split('|', StringSplitOptions.RemoveEmptyEntries)
        .FirstOrDefault(),
                    ProductPrice = ci.Product.Price,
                    Quantity = ci.Quantity,
                    Category = ci.Product.Category?.Name,
                    StockQuantity = ci.Product.StockQuantity
                }).ToList() ?? new List<CartItemVm>()

            };

            return (cartVM, null);
        }
        catch (Exception ex)
        {
            return (new CartVm { UserId = userId }, ex.Message);
        }
    }

    public async Task<(bool, string?)> AddToCart(string userId, AddToCartVm model)
    {
        try
        {
            // Get or create user's cart
            var (cart, cartError) = _cartRepo.GetByUserId(userId);
            if (!string.IsNullOrEmpty(cartError))
                return (false, cartError);

            if (cart == null)
            {
                var newCart = new Cart(userId, userId);
                var (created, createError) = _cartRepo.Create(newCart);
                if (!created)
                    return (false, createError);

                cart = newCart;
            }

            // Get product
            var (product, productError) = await _productRepo.GetById(model.ProductId);
            if (!string.IsNullOrEmpty(productError) || product == null)
                return (false, "Product not found");

            if (product.StockQuantity <= 0)
                return (false, "This product is out of stock.");

            // Check if item already exists in cart
            var (existingItem, _) = _cartItemRepo.GetByCartAndProduct(cart.CartId, model.ProductId);
            var existingQuantity = existingItem?.Quantity ?? 0;

            // Calculate total desired quantity
            var totalQuantity = existingQuantity + model.Quantity;

            if (totalQuantity > product.StockQuantity)
                return (false, $"Only {product.StockQuantity} items available.");

            if (existingItem != null)
            {
                existingItem.Update(totalQuantity, userId);
                return _cartItemRepo.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem(model.ProductId, model.Quantity, userId)
                {
                    CartId = cart.CartId
                };
                return _cartItemRepo.Create(cartItem);
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }


    public (bool, string?) UpdateCartItem(string userId, int cartItemId, int quantity)
    {
        try
        {
            var (cartItem, error) = _cartItemRepo.GetById(cartItemId);
            if (!string.IsNullOrEmpty(error) || cartItem == null)
                return (false, "Cart item not found");

            if (cartItem.Cart == null)
                return (false, "Cart not loaded");

            if (string.IsNullOrEmpty(cartItem.Cart.UserId))
                return (false, "Cart ownership cannot be verified");

            // Verify ownership
            if (cartItem.Cart.UserId != userId)
                return (false, "Unauthorized");

            if (quantity <= 0)
                return _cartItemRepo.Delete(cartItemId);

            // Check stock
            if (cartItem.Product == null)
                return (false, "Product details not loaded");

            if (cartItem.Product.StockQuantity < quantity)
                return (false, "Insufficient stock");

            cartItem.Update(quantity, userId);
            return _cartItemRepo.Update(cartItem);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public (bool, string?) RemoveFromCart(string userId, int cartItemId)
    {
        try
        {
            // Get user's cart
            var (cart, cartError) = _cartRepo.GetByUserId(userId);
            if (!string.IsNullOrEmpty(cartError) || cart == null)
                return (false, "Cart not found");

            var items = cart.cartItems?.Where(ci => !ci.IsDeleted).ToList() ?? new List<CartItem>();

            // If only one item in cart → clear whole cart
            if (items.Count == 1 && items.First().Cartitem == cartItemId)
            {
                return _cartRepo.ClearCart(userId);
            }

            // Otherwise delete the single item
            var (cartItem, error) = _cartItemRepo.GetById(cartItemId);
            if (!string.IsNullOrEmpty(error) || cartItem == null)
                return (false, "Cart item not found");

            if (cartItem.Cart == null)
                return (false, "Cart not loaded");

            if (cartItem.Cart.UserId != userId)
                return (false, "Unauthorized");

            return _cartItemRepo.Delete(cartItemId);
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
            return _cartRepo.ClearCart(userId);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public (int, string?) GetCartItemCount(string userId)
    {
        try
        {
            var (cart, error) = _cartRepo.GetByUserId(userId);
            if (!string.IsNullOrEmpty(error) || cart == null)
                return (0, null);

            var count = cart.cartItems?.Where(ci => !ci.IsDeleted).Sum(ci => ci.Quantity) ?? 0;
            return (count, null);
        }
        catch (Exception ex)
        {
            return (0, ex.Message);
        }
    }
}
