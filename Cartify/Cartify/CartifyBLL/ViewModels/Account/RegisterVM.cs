using CartifyDAL.Enum.User.userGender;
using System.ComponentModel.DataAnnotations;

namespace CartifyBLL.ViewModels.Account
{
    public class RegisterVM
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Required.")]
        [MaxLength(100)]
        public string FName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Required.")]
        [MaxLength(100)]
        public string LName { get; set; }

        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Required.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Weak Password")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password & Confirmed Password are not matched.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "You must agree to the terms and privacy policy")]
        public bool AgreeToTerms { get; } = true;
    }
}
