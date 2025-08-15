
using AutoMapper;
using CartifyBLL.Services.ProductReviewServices.Abstraction;
using CartifyBLL.ViewModels.Products.ProductReview;
using CartifyDAL.Entities.product;
using CartifyDAL.Repo.ProductReviewRepo.Abstraction;

namespace CartifyBLL.Services.ProductReviewServices.Impelementation
{
    public class ProductReviewServices : IProductReviewServices
    {
        private readonly IProductReviewRepo productReviewRepo;
        private readonly IMapper mapper;

        public ProductReviewServices(IProductReviewRepo productReviewRepo, IMapper mapper)
        {
            this.productReviewRepo = productReviewRepo;
            this.mapper = mapper;
        }

        public async Task<(bool, string?)> AddReviewAsync(CreateProductReview createProductReview)
        {
            try
            {
                var review = mapper.Map<ProductReview>(createProductReview);
                return await productReviewRepo.AddReviewAsync(review);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task<(List<ProductReviewDTO>, string?)> GetReviewByProductIdAsync(int productId)
        {
            try
            {
                var reviews = await productReviewRepo.GetReviewsByProductIdAsync(productId);
                if (reviews.Item2 != null)
                    return (null, reviews.Item2);

                var reviewDto = mapper.Map<List<ProductReviewDTO>>(reviews.Item1);
                return (reviewDto, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }

        }
    }
}
