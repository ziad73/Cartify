using AutoMapper;
using Cartify.DAL.DataBase;
using CartifyBLL.Services.CartService.Abstraction;
using CartifyBLL.Services.WishlistService.Abstraction;
using CartifyBLL.ViewModels.Cart;
using CartifyBLL.ViewModels.Wishlist;
using CartifyDAL.Entities.Wishlist;
using CartifyDAL.Repo.productRepo.Abstraction;
using CartifyDAL.Repo.WishlistRepo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyBLL.Services.WishlistService.Implementation;

public class WishlistService : IWishlistService
{
     private readonly IWishlistRepo _wishlistRepo;
        private readonly IWishlistItemRepo _wishlistItemRepo;
        private readonly IProductRepo _productRepo;
        private readonly ICartService _cartService;

        public WishlistService(
            IWishlistRepo wishlistRepo,
            IWishlistItemRepo wishlistItemRepo,
            IProductRepo productRepo,
            ICartService cartService)
        {
            _wishlistRepo = wishlistRepo;
            _wishlistItemRepo = wishlistItemRepo;
            _productRepo = productRepo;
            _cartService = cartService;
        }

        public (WishlistVm, string?) GetUserWishList(string userId)
        {
            try
            {
                var (wishlist, error) = _wishlistRepo.GetByUserId(userId);
                if (!string.IsNullOrEmpty(error))
                    return (new WishlistVm { UserId = userId }, error);

                if (wishlist == null)
                {
                    var newWishlist = new CartifyDAL.Entities.Wishlist.Wishlist(userId, userId);
                    var (created, createErr) = _wishlistRepo.Create(newWishlist);
                    if (!created) return (new WishlistVm { UserId = userId }, createErr);
                    wishlist = newWishlist;
                }

                var items = wishlist.Items?
                    .Where(wi => !wi.IsDeleted && wi.Product != null)
                    .Select(wi => new WishlistItemVm
                    {
                        WishListId = wishlist.WishListId,
                        ProductId = wi.ProductId,
                        ProductName = wi.Product.Name,
                        ProductImageUrl = wi.Product.ImageUrl,
                        ProductPrice = wi.Product.Price,
                        Category = wi.Product.Category?.Name,
                        StockQuantity = wi.Product.StockQuantity,
                        IsAvailable = wi.Product.StockQuantity > 0,
                        AddedOn = wi.CreatedOn
                    }).ToList() ?? new List<WishlistItemVm>();

                return (new WishlistVm
                {
                    UserId = userId,
                    Items = items
                }, null);
            }
            catch (Exception ex)
            {
                return (new WishlistVm { UserId = userId }, ex.Message);
            }
        }

        public (bool, string?) AddToWishList(string userId, int productId)
        {
            try
            {
                var (product, perr) = _productRepo.GetById(productId);
                if (!string.IsNullOrEmpty(perr) || product == null)
                    return (false, "Product not found");

                var (wishlist, werr) = _wishlistRepo.GetByUserId(userId);
                if (!string.IsNullOrEmpty(werr))
                    return (false, werr);

                if (wishlist == null)
                {
                    wishlist = new CartifyDAL.Entities.Wishlist.Wishlist(userId, userId);
                    var (created, cerr) = _wishlistRepo.Create(wishlist);
                    if (!created) return (false, cerr);
                }

                var (existing, _) = _wishlistItemRepo.GetByWishlistAndProduct(wishlist.WishListId, productId);
                if (existing != null) return (false, "Product already in wishlist");

                var item = new CartifyDAL.Entities.Wishlist.WishlistItem(wishlist.WishListId, productId, userId);
                return _wishlistItemRepo.Create(item);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) RemoveFromWishList(string userId, int productId)
        {
            try
            {
                var (wishlist, _) = _wishlistRepo.GetByUserId(userId);
                if (wishlist == null) return (false, "Wishlist not found");

                var (item, ierr) = _wishlistItemRepo.GetByWishlistAndProduct(wishlist.WishListId, productId);
                if (!string.IsNullOrEmpty(ierr) || item == null)
                    return (false, "Item not found");

                return _wishlistItemRepo.Delete(item.Id);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) ClearWishList(string userId)
        {
            try
            {
                var (wishlist, werr) = _wishlistRepo.GetByUserId(userId);
                if (!string.IsNullOrEmpty(werr) || wishlist == null)
                    return (false, "Wishlist not found");

                return _wishlistItemRepo.DeleteByWishlistId(wishlist.WishListId);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) MoveToCart(string userId, int productId)
        {
            try
            {
                // Build cart model from wishlist product
                var addToCartModel = new CartifyBLL.ViewModels.Cart.AddToCartVm
                {
                    ProductId = productId,
                    Quantity = 1 // default 1 item from wishlist
                };

                // 1) Add to cart
                var (added, cartError) = _cartService.AddToCart(userId, addToCartModel);
                if (!added)
                    return (false, cartError ?? "Failed to add to cart");

                // 2) Remove from wishlist
                var (removed, removeError) = RemoveFromWishList(userId, productId);
                if (!removed)
                    return (false, removeError ?? "Added to cart but failed to remove from wishlist");

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (int, string?) GetWishListCount(string userId)
        {
            try
            {
                var (wishlist, _) = _wishlistRepo.GetByUserId(userId);
                if (wishlist == null) return (0, null);

                return _wishlistItemRepo.CountByWishlistId(wishlist.WishListId);
            }
            catch (Exception ex)
            {
                return (0, ex.Message);
            }
        }

        public (bool, string?) IsInWishList(string userId, int productId)
        {
            try
            {
                var (wishlist, _) = _wishlistRepo.GetByUserId(userId);
                if (wishlist == null) return (false, null);

                var (item, _) = _wishlistItemRepo.GetByWishlistAndProduct(wishlist.WishListId, productId);
                return (item != null, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
