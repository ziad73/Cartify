using CartifyDAL.Entities.order;
using CartifyDAL.Enum.User.userGender;
using CartifyDAL.Enum.User.userGender;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CartifyDAL.Entities.user
{
    public class User : IdentityUser
    {
        public User(string fName, string type, UserGender s,DateTime Bdate)
        {
            FullName = fName;
            JoinDate = DateTime.Now;
            IsDeleted = false;
            Gender = s;
            BirthDate = Bdate;
        }
        public User()
        {
            JoinDate = DateTime.Now;
        }


        public string FullName { get; set; }

        public UserGender Gender { get;  set; }
        public DateTime BirthDate { get;  set; }

        public string? AvatarUrl { get; set; }

        // Navigation Properties
        public List<UserPayment>? UserPayments { get; private set; }
        public virtual ICollection<UserAddress> Addresses { get; set; }
        public List<Order>? Orders { get;  set; }

        // Audit Fields
        public DateTime JoinDate { get;  set; }
        public bool IsDeleted { get;  set; }
        public DateTime? DeletedOn { get;  set; }
        public string? DeletedBy { get;  set; }

        // Verification Fields
        public string? VerificationCode { get; set; }
        public DateTime? CodeSentAt { get; set; }
        public bool IsEmailVerified { get; set; } = false;


    }

}