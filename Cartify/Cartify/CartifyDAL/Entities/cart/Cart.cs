using CartifyDAL.Entities.productCart;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CartifyDAL.Entities.user;

namespace CartifyDAL.Entities.cart
{
    public class Cart
    { // commit
        public Cart(string createdBy ,  string? userId = null)
        {
            UserId = userId;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = false;

        }

        [Key]
        public int CartId { get; private set; }

        public List<ProductCart>? productCarts { get; private set; }
        public List<CartItem>? cartItems { get; private set; }

        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string? ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        
        // added new
        public string? UserId { get; private set; }
        
        [ForeignKey(nameof(UserId))]
        public User? User { get; private set; }

        public void Delete(string deletedBy)
        {
            IsDeleted = true;
            DeletedBy = deletedBy;
            DeletedOn = DateTime.Now;
        }
    }
}
