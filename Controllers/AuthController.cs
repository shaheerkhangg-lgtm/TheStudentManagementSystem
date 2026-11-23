using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheStudentManagementSystem.Data;
using TheStudentManagementSystem.Models;

namespace TheStudentManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        // Opens the Login page
        [HttpGet]
        public IActionResult Login()
        {
            // Already logged in? Go straight to the dashboard
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        // Handles the Login form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = model.Email.Trim().ToLower();

            // Look for the user in the database
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                // Compare the typed password with the saved hash
                var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    // Make the login cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, string.IsNullOrEmpty(user.Role) ? AppRoles.Pending : user.Role)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity));

                    return RedirectToAction("Welcome", "Dashboard");
                }
            }

            // Same message for a wrong email or a wrong password
            ModelState.AddModelError("Password", "Email or password is incorrect.");
            return View(model);
        }

        // Logs the user out
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Shown when a user has no permission for a page (NEW)
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Opens the Sign Up page
        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }

        // Handles the Sign Up form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signin(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = model.Email.Trim().ToLower();

            // Stop two users with the same email
            if (_db.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName.Trim(),
                Email = email,
                Role = AppRoles.Pending
            };

            // Scramble the password before saving it
            user.PasswordHash = _hasher.HashPassword(user, model.Password);

            try
            {
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }
}