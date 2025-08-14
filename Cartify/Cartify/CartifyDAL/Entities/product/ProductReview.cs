using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartifyDAL.Entities.product
{
    public class ProductReview
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public int ProductId { get; private set; }

        [MaxLength(1000)]
        public string Reviews { get; private set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; private set; }
        [Required]
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string? ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }

    }
}
