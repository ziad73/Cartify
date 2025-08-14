using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CartifyDAL.Entities.product;

namespace CartifyDAL.Entities.order
{
    public class OrderItem
    {
        public OrderItem(int quantity, double price, double discount, string createdBy)
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
        public int OrderId { get; set; }

        public int Quantity { get; private set; }
        public double Price { get; private set; }
        public double Discount { get; private set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        [Required]
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string? ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        public int ProductId { get; set; }
        
        // NEW — navigation to Product entity
        public Product Product { get; set; }  // make sure namespace matches your Product entity

        // Needed by EF
        public OrderItem() { }

        public void Update(int quantity, double price, double discount, string modifiedBy)
        {

            Quantity = quantity;
            Price = price;
            Discount = discount;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
        }

        public void Delete(string deletedBy)
        {
            IsDeleted = true;
            DeletedBy = deletedBy;
            DeletedOn = DateTime.Now;
        }
    }
}