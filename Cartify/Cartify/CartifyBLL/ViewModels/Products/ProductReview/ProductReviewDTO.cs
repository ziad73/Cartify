namespace CartifyBLL.ViewModels.Products.ProductReview
{
    public class ProductReviewDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ReviewerName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
