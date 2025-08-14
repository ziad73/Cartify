
using CartifyBLL.ViewModels.Products.ProductReview;

namespace CartifyBLL.Services.ProductReviewServices.Abstraction
{
    public interface IProductReviewServices
    {
        Task<(bool, string?)> AddReviewAsync(CreateProductReview createProductReview);
        Task<(List<ProductReviewDTO>, string?)> GetReviewByProductIdAsync(int productId);
        
    }
}
