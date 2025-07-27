using CartifyDAL.Entities.user;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartifyDAL.Entities.order
{
    public class Order
    {
        [Key]
        public int OrderId { get; private set; }

        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; private set; }
        [Required]
        public DateTime OrderDate { get; private set; }
        [MaxLength(100)]
        public string ShippingMethod { get; private set; }
        [MaxLength(100)]
        public string TrackingNumber { get; private set; }
        public decimal ShippingCost { get; private set; }
        public decimal Tax { get; private set; }
        public int? UserId { get; private set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; private set; }
        public List<OrderItem>? OrderItems { get; private set; }
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
