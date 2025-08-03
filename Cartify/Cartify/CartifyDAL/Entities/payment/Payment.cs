using CartifyDAL.Entities.user;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CartifyDAL.Entities.payment
{ //test
    public class Payment
    {
        public Payment(decimal amount, string createdBy)
        {
            Amount = amount;
            PaymentDate = DateTime.Now;
            IsDeleted = false;
        }
        public Payment() { }
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
        public bool IsDeleted { get; private set; }
        public string? DeletedBy { get; private set; }
    }
}
