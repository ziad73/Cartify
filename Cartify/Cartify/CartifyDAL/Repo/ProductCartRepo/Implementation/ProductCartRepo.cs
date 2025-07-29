
using Cartify.DAL.DataBase;
using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.productCart;
using CartifyDAL.Repo.ProductCartRepo.Abstraction;

namespace CartifyDAL.Repo.ProductCartRepo.Implementation
{
    public class ProductCartRepo : IProductCartRepo
    {
        private readonly CartifyDbContext db;

        public ProductCartRepo(CartifyDbContext db)
        {
            this.db = db;
        }
        public (bool, string?) Create(ProductCart productCart)
        {
            try
            {
                db.ProductCart.Add(productCart);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) Delete(int productCartId)
        {
            try
            {
                var existproductCart = db.ProductCart.FirstOrDefault(a => a.ProductId == productCartId && !a.IsDeleted);
                if (existproductCart == null)
                {
                    return (false, "Product Cart not found");
                }
                existproductCart.Delete(existproductCart.DeletedBy);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<ProductCart>, string?) GetAll()
        {
            try
            {
                var productCart = db.ProductCart.Where(a => !a.IsDeleted).ToList();
                return (productCart, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (ProductCart, string?) GetById(int productCartId)
        {
            try
            {
                var productCart = db.ProductCart.FirstOrDefault(a => a.ProductId == productCartId && !a.IsDeleted);
                if (productCart == null)
                {
                    return (null, "product Cart not found");
                }
                return (productCart, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (bool, string?) Update(ProductCart productCart)
        {
            try
            {
                var existingPCart = db.ProductCart.FirstOrDefault(a => a.ProductId == productCart.ProductId && !a.IsDeleted);
                if (existingPCart == null)
                {
                    return (false, "Cart item not found");
                }
                existingPCart.Update(productCart.ModifiedBy);
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
