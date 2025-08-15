using CartifyDAL.Entities.user;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartifyDAL.Entities.product
{
    public class ProductReview
    {
        public ProductReview(int productId, string userId, string reviewerName, string? comment, int rating)
        {
            ReviewerName = reviewerName;
            Comment = comment;
            Rating = rating;
            ProductId = productId;
            UserId = userId;
            CreatedOn = DateTime.Now;
        }
        public ProductReview()
        {
            
        }
        [Key]
        public int Id { get; private set; }

        public int ProductId { get; private set; }
        public string UserId { get; private set; }
        public string ReviewerName { get; private set; }

        [MaxLength(500)]
        public string? Comment { get; private set; }
        [Required]
        public int Rating { get; private set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; private set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime? CreatedOn { get; private set; }
        public string? ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public bool? IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }

    }
}
