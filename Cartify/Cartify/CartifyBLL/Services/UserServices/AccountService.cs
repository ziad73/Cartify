using CartifyBLL.ViewModels.Account;
using CartifyDAL.Entities.user;
using CartifyDAL.Repo.userRepo.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CartifyBLL.Services.UserServices
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepo userRepo;
        private readonly SignInManager<User> signInManager;
        private readonly EmailSender emailSender;

        public AccountService(IUserRepo userRepo, SignInManager<User> signInManager, EmailSender emailSender)
        {
            this.userRepo = userRepo;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        public async Task<(bool Success, string? ErrorMessage, bool IsEmailNotConfirmed)> LoginAsync(LoginVM model)
        {
            var user = await userRepo.GetByEmailAsync(model.Email);
            if (user == null)
                return (false, "Email or Password is invalid.", false);

            // Allow Admin login without email verification
            if (!user.IsEmailVerified)
            {
                var roles = await userRepo.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                    return (false, null, true); // Only non-admins require verification
            }


            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return (true, null, false);

            return (false, "Email or Password is invalid.", false);
        }


        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterVM model)
        {
            var user = new User
            {
                FullName = $"{model.FName} {model.LName}",
                PhoneNumber = model.Phone,
                Email = model.Email,
                UserName = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.Email.ToUpper(),
            };

            var success = await userRepo.AddAsync(user, model.Password);
            if (!success)
                return (false, "Failed to create user.");

            var code = new Random().Next(100000, 999999).ToString();

            // Store the code BEFORE sending it
            user.VerificationCode = code;
            user.CodeSentAt = DateTime.UtcNow;
            await userRepo.UpdateAsync(user);

            // Now send the correct stored code
            await emailSender.SendVerificationCodeAsync(user.Email, code);

            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> ConfirmEmailCodeAsync(EmailCodeVM model)
        {
            var user = await userRepo.GetByEmailAsync(model.Email);
            if (user == null)
                return (false, "User not found.");

            if (user.CodeSentAt == null || user.CodeSentAt.Value.AddMinutes(10) < DateTime.UtcNow)
                return (false, "Verification code expired.");

            if (user.VerificationCode?.Trim() != model.Code?.Trim())
                return (false, "Invalid verification code.");

            user.IsEmailVerified = true;
            user.VerificationCode = null; // clear code
            user.CodeSentAt = null;
            await userRepo.UpdateAsync(user);

            return (true, null);
        }


        public async Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(ResetPasswordVM model)
        {
            var user = await userRepo.GetByEmailAsync(model.Email);
            if (user == null)
                return (false, "User not found.");

            var removed = await userRepo.RemovePasswordAsync(user);
            if (!removed)
                return (false, "Failed to remove existing password.");

            var added = await userRepo.AddPasswordAsync(user, model.NewPassword);
            if (!added)
                return (false, "Failed to add new password.");

            return (true, null);
        }


        public async Task<bool> SendVerificationCodeAsync(string email, string purpose)
        {
            var user = await userRepo.GetByEmailAsync(email);
            if (user == null)
                return false;

            var code = new Random().Next(100000, 999999).ToString();
            user.VerificationCode = code;
            user.CodeSentAt = DateTime.UtcNow;
            await userRepo.UpdateAsync(user);

            await emailSender.SendVerificationCodeAsync(email, code);
            return true;
        }

        public bool VerifyEmailCode(string email, string code, string storedCode, string purpose)
        {
            return code == storedCode;
        }

    }
}
