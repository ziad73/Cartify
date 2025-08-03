using CartifyDAL.Entities.order;
using CartifyDAL.Enum.User.userSex;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CartifyDAL.Entities.user
{
    public class User : IdentityUser
    {
        public User(string fName, string type, UserSex s,DateTime Bdate)
        {
            FullName = fName;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
            Sex = s;
            BirthDate = Bdate;
        }
        public User() { }


        public string FullName { get; set; }

        public UserSex Sex { get;  set; }
        public DateTime BirthDate { get;  set; }

        public string? PostalCode { get; set; }

        // Navigation Properties
        public List<UserPayment>? UserPayments { get; private set; }
        public List<UserAddress>? Addresses { get; private set; }
        public List<Order>? Orders { get; private set; }

        // Audit Fields
        public DateTime CreatedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }

        // Verification Fields
        public string? VerificationCode { get; set; }
        public DateTime? CodeSentAt { get; set; }
        public bool IsEmailVerified { get; set; } = false;


    }

}