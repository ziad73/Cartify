using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartifyDAL.Entities.productCart
{
    public class ProductCart
    {

        public ProductCart(string createdBy)
        {
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
        }
        public int CartId { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(CartId))]
        public Cart cart { get; private set; }
        [ForeignKey(nameof(ProductId))]
        public Product product { get; private set; }

        [Required]
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }


}
