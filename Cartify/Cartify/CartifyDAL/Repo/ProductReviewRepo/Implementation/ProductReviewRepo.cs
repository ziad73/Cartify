
using Cartify.DAL.DataBase;
using CartifyDAL.Entities.product;
using CartifyDAL.Repo.productRepo.Abstraction;
using CartifyDAL.Repo.ProductReviewRepo.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;

namespace CartifyDAL.Repo.ProductReviewRepo.Implementation
{
    public class ProductReviewRepo : IProductReviewRepo
    {
        private readonly CartifyDbContext cartifyDbContext;

        public ProductReviewRepo(CartifyDbContext cartifyDbContext)
        {
            this.cartifyDbContext = cartifyDbContext;
        }

        public async Task<(bool, string?)> AddReviewAsync(ProductReview review)
        {
            try
            {
                await cartifyDbContext.ProductReview.AddAsync(review);
                await cartifyDbContext.SaveChangesAsync();
                return (true, null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return (false, ex.Message);
            }
        }

        public async Task<(List<ProductReview>, string?)> GetReviewsByProductIdAsync(int productId)
        {
            try
            {
                var review = await cartifyDbContext.ProductReview.Where(a => a.ProductId == productId).OrderByDescending(r => r.CreatedOn).ToListAsync();
                return (review, null);
            }
            catch(Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
}
