using CartifyBLL.Services.UserServices;
using CartifyBLL.ViewModels.Account;
using CartifyDAL.Entities.user;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartifyPLL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AccountController(
    IAccountService accountService,
    SignInManager<User> signInManager,
    UserManager<User> userManager)
        {
            this.accountService = accountService;
            this.signInManager = signInManager;
            this.userManager = userManager;
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("LoginView");
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            // Try to log in the user with external login info
            var result = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            // Try to get email from external provider
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email;

            if (email == null)
            {
                ModelState.AddModelError(string.Empty, "Email not received from external provider.");
                return View("LoginView");
            }

            // Check if user already exists by email
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                // Link the external login and sign in
                await userManager.AddLoginAsync(existingUser, info);
                await signInManager.SignInAsync(existingUser, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            // User does not exist, create a new one
            var newUser = new User
            {
                UserName = email,
                Email = email,
                FullName = name
            };

            var createResult = await userManager.CreateAsync(newUser);
            if (createResult.Succeeded)
            {
                await userManager.AddLoginAsync(newUser, info);
                await signInManager.SignInAsync(newUser, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View("LoginView");
        }

    }
}
