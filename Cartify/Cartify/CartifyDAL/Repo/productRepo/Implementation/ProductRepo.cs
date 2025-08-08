using Cartify.DAL.DataBase;
using CartifyDAL.Entities.category;
using CartifyDAL.Entities.product;
using CartifyDAL.Repo.productRepo.Abstraction;

namespace CartifyDAL.Repo.ProductRepo.Implementation
{
    public class ProductRepo : IProductRepo
    {
        private readonly CartifyDbContext db;

        public ProductRepo(CartifyDbContext db)
        {
            this.db = db;
        }
        public (bool, string?) Create(Product product)
        {
            try
            {
                db.Product.Add(product);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) Delete(int productId)
        {
            try
            {
                var product = db.Product.FirstOrDefault(a => a.ProductId == productId);
                if (product == null)
                {
                    return (false, "Product not found");
                }
                product.Delete(product.DeletedBy);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<Product>, string?) GetAll()
        {
            try
            {
                var products = db.Product
                    .Where(a => !a.IsDeleted)
                    .ToList();
                return (products, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (Product, string?) GetById(int productId)
        {
            try { 
            var product = db.Product
                   .FirstOrDefault(a => a.ProductId == productId && !a.IsDeleted);
            if (product == null)
            {
                return (null, "Product not found");
            }
            return (product, null);
        }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
}

        public (bool, string?) Update(Product product)
        {
            try
            {
                var existingProduct = db.Product.FirstOrDefault(a => a.ProductId == product.ProductId && !a.IsDeleted);
                if (existingProduct == null)
                {
                    return (false, "Product not found");
                }
                existingProduct.Update(product.StockQuantity, product.Price, product.Description, product.IsActive , product.CategoryId ,product.ModifiedBy);
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
