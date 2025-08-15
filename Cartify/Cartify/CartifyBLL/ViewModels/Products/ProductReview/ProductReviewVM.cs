using CartifyBLL.ViewModels.Product;

namespace CartifyBLL.ViewModels.Products.ProductReview
{
    public class ProductReviewVM
    {
        public ProductDTO Product { get; set; }
        public List<ProductReviewDTO> Reviews { get; set; }
        public CreateProductReview create { get; set; }
    }
}
