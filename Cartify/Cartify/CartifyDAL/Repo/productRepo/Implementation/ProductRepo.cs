using Cartify.DAL.DataBase;
using CartifyDAL.Entities.category;
using CartifyDAL.Entities.product;
using CartifyDAL.Repo.productRepo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.ProductRepo.Implementation
{
    public class ProductRepo : IProductRepo
    {
        private readonly CartifyDbContext db;

        public ProductRepo(CartifyDbContext db)
        {
            this.db = db;
        }
        public async Task<(bool, string?)> Create(Product product)
        {
            try
            {
                await db.Product.AddAsync(product);
                await db.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string?)> Delete(int productId)
        {
            try
            {
                var product = await db.Product.FirstOrDefaultAsync(a => a.ProductId == productId);
                if (product == null)
                {
                    return (false, "Product not found");
                }
                product.Delete(product.DeletedBy);
                await db.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(List<Product>, string?)> GetAll()
        {
            try
            {
                var products = await db.Product.Where(a => !a.IsDeleted && a.IsActive).Include(p => p.Category).ToListAsync();
                return (products, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(Product, string?)> GetById(int productId)
        {
            try { 
            var product = await db.Product.AsNoTracking().FirstOrDefaultAsync(a => a.ProductId == productId && !a.IsDeleted && a.IsActive);
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

        public async Task<(bool, string?)> Update(Product product)
        {
            try
            {
                var existingProduct = await db.Product.FirstOrDefaultAsync(a => a.ProductId == product.ProductId && !a.IsDeleted && a.IsActive);
                if (existingProduct == null)
                {
                    return (false, "Product not found");
                }
                existingProduct.Update(product.Name, product.StockQuantity, product.Price, product.Description, product.ImageUrl, product.IsActive , product.CategoryId ,product.ModifiedBy);
                await db.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
