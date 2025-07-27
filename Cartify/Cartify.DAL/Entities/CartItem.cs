﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cartify.DAL.Entities
{
    public class CartItem
    {
        public int Cartitem { get; private set; }
        public int CartId { get; private set; }
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
    }
}
