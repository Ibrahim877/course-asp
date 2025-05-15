using System.Security.Claims;
using Course_APP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Course_APP.Models;

namespace Course_APP.Areas.panel.Controllers
{
    [Area("panel")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdStr == null || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Index", "Auth");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Logout", "Auth");
            }

            return View(user);
        }
    }
}
