using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartifyDAL.Entities.user.payment
{
    public class PaymentMethod
    {
        public PaymentMethod(string paymentMethodName, string createdBy)
        {
            PaymentMethodName = paymentMethodName;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
        }

        [Key]
        public int PaymentMethodId { get; private set; }

        [Required]
        [MaxLength(100)]
        public string PaymentMethodName { get; private set; }

        public List<Payment> Payments { get; private set; }
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
