
using System.ComponentModel.DataAnnotations;

namespace Cartify.DAL.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; private set; }

        public List<ProductCart>? productCarts { get; private set; }
        public List<CartItem>? cartItems { get; private set; }

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
