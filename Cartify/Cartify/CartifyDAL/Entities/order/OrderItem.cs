using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartifyDAL.Entities.order
{
    public class OrderItem
    {
        public OrderItem(int quantity, decimal price, decimal discount, string createdBy)
        {
            Quantity = quantity;
            Price = price;
            Discount = discount;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
        }

        [Key]
        public int OrderItemId { get; private set; }

        [Required]
        public int OrderId { get; private set; }

        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal Discount { get; private set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; private set; }

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
