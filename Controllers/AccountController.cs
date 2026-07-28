using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using PersonRegistrationMvc.Models;
using PersonRegistrationMvc.Services;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var user = await _userManager.FindByEmailAsync(viewModel.Email);

            
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Account",
                    new { email = user.Email, token = token }, protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Reset your password",
                    $"Click <a href='{resetLink}'>here</a> to reset your password. If you didn't request this, ignore this email.");
            }

            TempData["Message"] = "If that email is registered, a password reset link has been sent.";
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
            {
                // Same "don't reveal whether the account exists" principle as ForgotPassword.
                TempData["Message"] = "Your password has been reset.";
                return RedirectToAction("Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, viewModel.Token, viewModel.NewPassword);
            if (result.Succeeded)
            {
                TempData["Message"] = "Your password has been reset. You can log in now.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var user = new ApplicationUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                EmailConfirmed = true,
                FullName = viewModel.FullName,
                CreatedBy = GetCurrentUserId(),
                CreatedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, viewModel.Role);
                await _userManager.AddClaimAsync(user, new Claim("FullName", viewModel.FullName));
                return RedirectToAction("Index", "Dashboard");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModels = new List<UserListViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                viewModels.Add(new UserListViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "—"
                });
            }
            return View(viewModels);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserEditViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = roles.FirstOrDefault() ?? "User"
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var user = await _userManager.FindByIdAsync(viewModel.Id);
            if (user == null) return NotFound();

            bool isEditingSelf = user.Id == _userManager.GetUserId(User);
            var currentRoles = await _userManager.GetRolesAsync(user);

            if (isEditingSelf && !currentRoles.Contains(viewModel.Role))
            {
                ModelState.AddModelError("", "You cannot change your own role while logged in as this account.");
                return View(viewModel);
            }

            user.FullName = viewModel.FullName;
            user.UpdatedBy = GetCurrentUserId();
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var claims = await _userManager.GetClaimsAsync(user);
            var fullNameClaim = claims.FirstOrDefault(c => c.Type == "FullName");
            if (fullNameClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, fullNameClaim, new Claim("FullName", viewModel.FullName));
            }
            else
            {
                await _userManager.AddClaimAsync(user, new Claim("FullName", viewModel.FullName));
            }

            if (!currentRoles.Contains(viewModel.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, viewModel.Role);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == _userManager.GetUserId(User))
            {
                TempData["ErrorMessage"] = "You cannot delete your own account while logged in as it.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}