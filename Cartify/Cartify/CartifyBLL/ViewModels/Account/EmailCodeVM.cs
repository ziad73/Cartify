using System.ComponentModel.DataAnnotations;

namespace CartifyBLL.ViewModels.Account
{
    public class EmailCodeVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }

        public string Purpose { get; set; } = "Register"; // Default to Register
    }
}
