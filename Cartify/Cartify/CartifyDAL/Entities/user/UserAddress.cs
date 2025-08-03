using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CartifyDAL.Entities.user
{
    public class UserAddress
    {
        public UserAddress(string address, string createdBy)
        {
            Address = address;
            IsDeleted = false;
        }
        public UserAddress(){}

        [Key]
        public int Id { get; private set; }

        [Required]
        public string UserId { get; private set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; private set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; private set; }

        [Required]
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
    }
}
