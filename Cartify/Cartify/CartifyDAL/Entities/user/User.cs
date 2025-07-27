using CartifyDAL.Entities.order;
using System.ComponentModel.DataAnnotations;

namespace CartifyDAL.Entities.user
{
    public class User
    {
        public User(string fName, string lName, string type, string createdBy)
        {
            FName = fName;
            LName = lName;
            Type = type;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public User()
        {

        }

        [Key]
        public int UserId { get; private set; }

        [Required]
        [MaxLength(100)]
        public string FName { get; private set; }

        [MaxLength(100)]
        public string LName { get; private set; }
        public bool IsActive { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; private set; }

        public DateTime? LastLogin { get; private set; }

        // Navigation Properties
        public List<UserPayment>? UserPayments { get; private set; }
        public List<UserAddress>? Addresses { get; private set; }
        public List<Order>? Orders { get; private set; }

        // Audit Fields
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