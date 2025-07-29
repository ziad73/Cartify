
using Cartify.DAL.DataBase;
using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.category;
using CartifyDAL.Repo.CartRepo.Abstraction;

namespace CartifyDAL.Repo.CartRepo.Implementation
{
    public class CartRepo : ICartRepo
    {
        private readonly CartifyDbContext db;

        public CartRepo(CartifyDbContext db)
        {
            this.db = db;
        }

        public (bool, string?) Create(Cart cart)
        {
            try
            {
                db.Cart.Add(cart);
                db.SaveChanges();
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
                var existingCart = db.Cart.FirstOrDefault(a => a.CartId == cartId && !a.IsDeleted);
                if (existingCart == null)
                {
                    return (false, "Cart item not found");
                }
                existingCart.Delete(existingCart.DeletedBy);
                db.SaveChanges();
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
                var cart = db.Cart.Where(a => !a.IsDeleted).ToList();
                return (cart, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (Cart, string?) GetById(int cartId)
        {
            try
            {
                var cart = db.Cart.FirstOrDefault(a => a.CartId == cartId && !a.IsDeleted);
                if (cart == null)
                {
                    return (null, "Order item not found");
                }
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
                var existingCart = db.Cart.FirstOrDefault(a => a.CartId == cart.CartId && !a.IsDeleted);
                if (existingCart == null)
                {
                    return (false, "Cart item not found");
                }
                existingCart.Update(cart.ModifiedBy);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
