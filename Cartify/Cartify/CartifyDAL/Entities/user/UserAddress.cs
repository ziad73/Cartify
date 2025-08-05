using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CartifyDAL.Entities.user
{
    public class UserAddress
    {
        public UserAddress()
        {
            IsDeleted = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; private set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public bool IsDefault { get; set; } 


        [ForeignKey(nameof(UserId))]
        public User User { get; private set; }

        [Required]
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
    }
}
