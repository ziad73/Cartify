using CartifyBLL.Services.UserServices;
using CartifyBLL.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("LoginView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View("LoginView", model);

            var (success, error, isEmailNotConfirmed) = await accountService.LoginAsync(model);

            if (isEmailNotConfirmed)
            {
                TempData["Email"] = model.Email;
                TempData["VerificationPurpose"] = "Register"; // Or "Reset" if this is from forgot password

                return View("EmailCodeVerificationView", new EmailCodeVM
                {
                    Email = model.Email,
                    Purpose = "Register"
                });
            }

            if (success)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", error);
            return View("LoginView", model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View("RegisterView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View("RegisterView", model);

            var (success, error) = await accountService.RegisterAsync(model);
            if (success)
            {
                TempData["Email"] = model.Email;
                TempData["VerificationPurpose"] = "Register";

                return View("EmailCodeVerificationView", new EmailCodeVM
                {
                    Email = model.Email,
                    Purpose = "Register"
                });
            }

            ModelState.AddModelError("", error);
            return View("RegisterView", model);
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View("VerifyEmailView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(VerifyEmailVM model)
        {
            if (!ModelState.IsValid)
                return View("VerifyEmailView", model);

            TempData["Email"] = model.Email;
            TempData["VerificationPurpose"] = "Reset";

            var sent = await accountService.SendVerificationCodeAsync(model.Email, "Reset");
            if (!sent)
            {
                ModelState.AddModelError("", "Failed to send verification code. Please check the email.");
                return View("VerifyEmailView", model);
            }

            return View("EmailCodeVerificationView", new EmailCodeVM
            {
                Email = model.Email,
                Purpose = "Reset"
            });
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("VerifyEmail");

            return View("ResetPasswordView", new ResetPasswordVM { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View("ResetPasswordView", model);

            var (success, error) = await accountService.ResetPasswordAsync(model);
            if (success)
                return RedirectToAction("Login");

            ModelState.AddModelError("", error);
            return View("ResetPasswordView", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailCodeVerification(EmailCodeVM model)
        {
            if (!ModelState.IsValid)
                return View("EmailCodeVerificationView", model);

            var result = await accountService.ConfirmEmailCodeAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View("EmailCodeVerificationView", model);
            }

            if (model.Purpose == "Reset")
                return RedirectToAction("ResetPassword", new { email = model.Email });

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
