using System.ComponentModel.DataAnnotations;

namespace CartifyBLL.ViewModels.Account
{
    public class VerifyEmailVM
    {
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress]
        public string Email { get; set; } 
        
    }
}
