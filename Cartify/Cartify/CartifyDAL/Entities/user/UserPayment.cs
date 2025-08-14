using CartifyDAL.Entities.payment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CartifyDAL.Entities.user
{
    public class UserPayment
    {
        public UserPayment()
        {
            IsDeleted = false;
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string UserId { get; private set; }

        [Required]
        public int PaymentId { get; private set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; private set; }

        [ForeignKey(nameof(PaymentId))]
        public Payment Payment { get; private set; }

        [Required]
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }

    }

}
