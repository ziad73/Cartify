using CartifyDAL.Entities.product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartifyDAL.Entities.category
{
    public class Category
    {
        [Key]
        public int CategoryId { get; private set; }
        public string Name { get; private set; }
        [MaxLength(1000)]
        public string? Description { get; private set; }
        public int? ParentCategoryId { get; private set; }

        [ForeignKey(nameof(ParentCategoryId))]
        public Category? Parentcategory { get; private set; }

        public List<Product> products { get; private set; }

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
