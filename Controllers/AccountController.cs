using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.Services;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPersonRepository _personRepository;
        private readonly IEmailSender _emailSender;
        private readonly Microsoft.Extensions.Localization.IStringLocalizer<SharedResource> _localizer;

        public AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<IdentityRole> roleManager,
    IPersonRepository personRepository,
    IEmailSender emailSender,
    Microsoft.Extensions.Localization.IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _personRepository = personRepository;
            _emailSender = emailSender;
            _localizer = localizer;
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private List<SelectListItem> GetRoleOptions()
        {
            return _roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem { Value = r.Id, Text = r.Name })
                .ToList();
        }

        private List<SelectListItem> GetUnlinkedPersonOptions(int? includePersonId = null)
        {
            var linkedPersonIds = _userManager.Users
                .Where(u => u.PersonId != null && u.PersonId != includePersonId)
                .Select(u => u.PersonId!.Value)
                .ToHashSet();

            return _personRepository.GetAll()
                .Where(p => !linkedPersonIds.Contains(p.PersonId))
                .OrderBy(p => p.FullName)
                .Select(p => new SelectListItem { Value = p.PersonId.ToString(), Text = p.FullName })
                .ToList();
        }

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

            ModelState.AddModelError("", _localizer["InvalidLoginAttempt"]);
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
        public IActionResult Register()
        {
            var viewModel = new RegisterViewModel
            {
                RoleOptions = GetRoleOptions(),
                PersonOptions = GetUnlinkedPersonOptions()
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            var role = await _roleManager.FindByIdAsync(viewModel.RoleId ?? "");

            if (role == null)
            {
                ModelState.AddModelError("RoleId", "Selected role no longer exists.");
            }
            else if (role.Name == "User")
            {
                if (viewModel.LinkExistingPerson)
                {
                    if (viewModel.PersonId == null || viewModel.PersonId <= 0)
                        ModelState.AddModelError("PersonId", "Please select a library member to link.");
                }
                else
                {
                    if (viewModel.Age == null) ModelState.AddModelError("Age", "Age is required.");
                    if (viewModel.Gender == null) ModelState.AddModelError("Gender", "Gender is required.");
                    if (string.IsNullOrWhiteSpace(viewModel.Major)) ModelState.AddModelError("Major", "Major is required.");
                }
            }

            if (!ModelState.IsValid)
            {
                viewModel.RoleOptions = GetRoleOptions();
                viewModel.PersonOptions = GetUnlinkedPersonOptions();
                return View(viewModel);
            }

            int? personId = null;

            if (role!.Name == "User")
            {
                if (viewModel.LinkExistingPerson)
                {
                    personId = viewModel.PersonId;
                }
                else
                {
                    var newPerson = new Person
                    {
                        FullName = viewModel.FullName,
                        Age = viewModel.Age!.Value,
                        Gender = viewModel.Gender!.Value,
                        Major = viewModel.Major!,
                        CreatedBy = GetCurrentUserId(),
                        CreatedAt = DateTime.UtcNow
                    };
                    _personRepository.Add(newPerson);
                    personId = newPerson.PersonId;
                }
            }

            var user = new ApplicationUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                EmailConfirmed = true,
                FullName = viewModel.FullName,
                PersonId = personId,
                CreatedBy = GetCurrentUserId(),
                CreatedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
                await _userManager.AddClaimAsync(user, new Claim("FullName", viewModel.FullName));
                return RedirectToAction("Index", "Dashboard");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            viewModel.RoleOptions = GetRoleOptions();
            viewModel.PersonOptions = GetUnlinkedPersonOptions();
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

            var roleNames = await _userManager.GetRolesAsync(user);
            var currentRoleName = roleNames.FirstOrDefault() ?? "User";
            var currentRole = await _roleManager.FindByNameAsync(currentRoleName);

            var viewModel = new UserEditViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                RoleId = currentRole?.Id,
                PersonId = user.PersonId,
                LinkExistingPerson = user.PersonId != null,
                RoleOptions = GetRoleOptions(),
                PersonOptions = GetUnlinkedPersonOptions(user.PersonId)
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.Id ?? "");
            if (user == null) return NotFound();

            var selectedRole = await _roleManager.FindByIdAsync(viewModel.RoleId ?? "");

            if (selectedRole == null)
            {
                ModelState.AddModelError("RoleId", "Selected role no longer exists.");
            }
            else if (selectedRole.Name == "User")
            {
                if (viewModel.LinkExistingPerson)
                {
                    if (viewModel.PersonId == null || viewModel.PersonId <= 0)
                        ModelState.AddModelError("PersonId", "Please select a library member to link.");
                }
                else
                {
                    if (viewModel.Age == null) ModelState.AddModelError("Age", "Age is required.");
                    if (viewModel.Gender == null) ModelState.AddModelError("Gender", "Gender is required.");
                    if (string.IsNullOrWhiteSpace(viewModel.Major)) ModelState.AddModelError("Major", "Major is required.");
                }
            }

            if (!ModelState.IsValid)
            {
                viewModel.RoleOptions = GetRoleOptions();
                viewModel.PersonOptions = GetUnlinkedPersonOptions(user.PersonId);
                return View(viewModel);
            }

            bool isEditingSelf = user.Id == _userManager.GetUserId(User);
            var currentRoles = await _userManager.GetRolesAsync(user);

            if (isEditingSelf && !currentRoles.Contains(selectedRole!.Name))
            {
                ModelState.AddModelError("", "You cannot change your own role while logged in as this account.");
                viewModel.RoleOptions = GetRoleOptions();
                viewModel.PersonOptions = GetUnlinkedPersonOptions(user.PersonId);
                return View(viewModel);
            }

            int? personId = null;

            if (selectedRole!.Name == "User")
            {
                if (viewModel.LinkExistingPerson)
                {
                    personId = viewModel.PersonId;
                }
                else
                {
                    var newPerson = new Person
                    {
                        FullName = viewModel.FullName,
                        Age = viewModel.Age!.Value,
                        Gender = viewModel.Gender!.Value,
                        Major = viewModel.Major!,
                        CreatedBy = GetCurrentUserId(),
                        CreatedAt = DateTime.UtcNow
                    };
                    _personRepository.Add(newPerson);
                    personId = newPerson.PersonId;
                }
            }

            user.FullName = viewModel.FullName;
            user.PersonId = personId;
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

            if (!currentRoles.Contains(selectedRole.Name))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, selectedRole.Name);
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