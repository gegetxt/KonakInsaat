using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class adminController : Controller
    {
        private readonly KonakInsaatContext _context;

        public adminController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa admin home'a yönlendir
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("AdminHome", "Home");
            }
            return View();
        }

        // POST: Admin/admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                TempData["Error"] = "Kullanıcı adı ve şifre gereklidir.";
                return View();
            }

            // Kullanıcıyı veritabanından bul
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.KullaniciAdi == kullaniciAdi && k.Aktif);

            if (kullanici == null)
            {
                TempData["Error"] = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            // Şifre kontrolü (SHA256 hash)
            string hashedPassword = HashPassword(sifre);
            
            // Şifre kontrolü
            if (kullanici.Sifre != hashedPassword)
            {
                // Şifre yanlışsa, T1 olarak güncelle (geliştirme ortamı için)
                if (kullaniciAdi == "admin" && sifre == "T1")
                {
                    kullanici.Sifre = hashedPassword;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    TempData["Error"] = "Kullanıcı adı veya şifre hatalı.";
                    return View();
                }
            }

            // Giriş başarılı - Claims oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciID.ToString()),
                new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
                new Claim(ClaimTypes.Email, kullanici.Email),
                new Claim(ClaimTypes.Role, kullanici.Rol)
            };

            if (!string.IsNullOrEmpty(kullanici.AdSoyad))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, kullanici.AdSoyad));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true, // "Beni Hatırla"
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            // Son giriş tarihini güncelle
            kullanici.SonGirisTarihi = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Giriş başarılı! Hoş geldiniz.";
            return RedirectToAction("AdminHome", "Home");
        }

        // GET/POST: Admin/admin/Logout
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Login");
        }

        // GET: Admin/admin/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Şifre hash fonksiyonu (SHA256) - Program.cs ile uyumlu
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(bytes).ToLower();
            }
        }
    }
}

