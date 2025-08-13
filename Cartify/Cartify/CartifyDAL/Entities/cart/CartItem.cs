using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CartifyDAL.Entities.product;

namespace CartifyDAL.Entities.cart
{
    public class CartItem
    {
        public CartItem() { }

        public CartItem(int productId, int quantity, string createdBy )
        {
            ProductId = productId;
            Quantity = quantity;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
           
        }
        public void Update(int quantity, string modifiedBy) // new
        {
            Quantity = quantity;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }

        [Key]
        public int Cartitem { get; private set; }
      
        public int CartId { get;  set; }
        public int Quantity { get; private set; }

        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }
        [Required]
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string? ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        // added new
        [Required]
        public int ProductId { get; private set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; private set; }

        public void Delete(string deletedBy)
        {
            IsDeleted = true;
            DeletedBy = deletedBy;
            DeletedOn = DateTime.Now;
        }
    }
}
