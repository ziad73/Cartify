using CartifyBLL.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartifyBLL.Services.UserServices
{
    public interface IAccountService
    {
        Task<(bool Success, string? ErrorMessage, bool IsEmailNotConfirmed)> LoginAsync(LoginVM model);
        Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterVM model);
        Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(ResetPasswordVM model);
        Task<bool> SendVerificationCodeAsync(string email, string purpose);
        Task<(bool Success, string? ErrorMessage)> ConfirmEmailCodeAsync(EmailCodeVM model);
        bool VerifyEmailCode(string email, string code, string storedCode, string purpose);
    }
}
