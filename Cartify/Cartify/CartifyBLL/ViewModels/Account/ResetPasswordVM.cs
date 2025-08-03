using System.ComponentModel.DataAnnotations;

namespace CartifyBLL.ViewModels.Account
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "New Password is Required.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Weak Password")]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "Password & Confirmed Password are not matched.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is Required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
