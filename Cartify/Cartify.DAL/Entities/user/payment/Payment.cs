using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CartifyDAL.Entities.user.payment
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; private set; }

        [Required]
        public decimal Amount { get; private set; }

        [Required]
        public DateTime PaymentDate { get; private set; }

        public int? PaymentMethodId { get; private set; }

        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod PaymentMethod { get; private set; }

        public List<UserPayment> UserPayments { get; private set; }
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
