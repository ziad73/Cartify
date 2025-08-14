using CartifyDAL.Entities.product;

namespace CartifyDAL.Repo.ProductReviewRepo.Abstraction
{
    public interface IProductReviewRepo
    {
        Task<(bool, string?)> AddReviewAsync(ProductReview review);
        Task<(List<ProductReview>, string?)> GetReviewsByProductIdAsync(int productId);
    }
}
