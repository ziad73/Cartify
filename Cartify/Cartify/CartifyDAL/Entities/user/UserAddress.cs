using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CartifyDAL.Entities.user
{
    public class UserAddress
    {
        public UserAddress(string address, string createdBy)
        {
            Address = address;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
        }
        public UserAddress()
        {

        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public int UserId { get; private set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; private set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; private set; }

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
