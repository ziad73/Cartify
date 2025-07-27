using CartifyDAL.Entities.category;
using CartifyDAL.Entities.order;
using CartifyDAL.Entities.productCart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartifyDAL.Entities.product
{
    public class Product
    {
        [Key]
        public int ProductId { get; private set; }
        public int? OrderId { get; private set; }

        [Required]
        public int CategoryId { get; private set; }

        public int StockQuantity { get; private set; }
        public decimal Price { get; private set; }

        [MaxLength(1000)]
        public string Description { get; private set; }

        public DateTime DateAdded { get; private set; }
        public DateTime? LastUpdated { get; private set; }

        public bool IsActive { get; private set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; private set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; private set; }

        public List<ProductReview> ProductReviews { get; private set; }
        [Required]
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string? ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }
        
        
        //M-TO-M Relationship
        public List<ProductCart>? productCarts { get; set; }    
    }
}
