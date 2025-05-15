using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_APP.Areas.panel.ViewModels.Auth;
using Course_APP.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Course_APP.Models;

namespace Course_APP.Areas.panel.Controllers
{
    [Area("panel")]
    public class AuthController : Controller
    {

        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reg()
        {
            return View("~/Areas/panel/Views/Auth/Reg.cshtml");
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View("~/Areas/panel/Views/Auth/Index.cshtml", model);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Email və ya şifrə yanlışdır.");
                return View("~/Areas/panel/Views/Auth/Index.cshtml", model);
            }


            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("Email", user.Email)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe, // Remember Me seçimi
                ExpiresUtc = model.RememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            return RedirectToAction("Index", "dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Auth");
        }


        [HttpPost]
        public async Task<IActionResult> Reg(RegVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Bu email artıq qeydiyyatdan keçib.");
                return View(model);
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newUser = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Password = hashedPassword,
                Role = model.Role
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();



            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
            new Claim(ClaimTypes.Name, model.Name),
            new Claim(ClaimTypes.Role, model.Role),
            new Claim("Email", model.Email)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false, // Remember Me seçimi
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);


            return RedirectToAction("Index", "dashboard");
        }
    }
}