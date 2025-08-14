
using System.ComponentModel.DataAnnotations;

namespace CartifyBLL.ViewModels.Products.ProductReview
{
    public class CreateProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }
        public string ReviewerName { get; set; }

    }
}
