using KonakInsaat.Data;
using KonakInsaat.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace KonakInsaat.Controllers
{
    public class AccountController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AccountController(KonakInsaatContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Admin area'sındaki login sayfasına yönlendir
            return RedirectToAction("Login", "admin", new { area = "Admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            // Admin area'sındaki login action'ına yönlendir
            return RedirectToAction("Login", "admin", new { area = "Admin" });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "admin", new { area = "Admin" });
        }
    }
}