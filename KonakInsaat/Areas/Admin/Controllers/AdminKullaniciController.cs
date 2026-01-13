using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;
using System.Security.Cryptography;
using System.Text;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminKullaniciController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AdminKullaniciController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminKullanici
        public async Task<IActionResult> Index()
        {
            var kullanicilar = await _context.Kullanicilar.ToListAsync();
            return View("AdminKullanici", kullanicilar);
        }

        // GET: Admin/AdminKullanici/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null) return NotFound();

            return View(kullanici);
        }

        // POST: Admin/AdminKullanici/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kullanici kullanici, string? yeniSifre)
        {
            if (id != kullanici.KullaniciID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutKullanici = await _context.Kullanicilar.FindAsync(id);
                    if (mevcutKullanici == null) return NotFound();

                    mevcutKullanici.KullaniciAdi = kullanici.KullaniciAdi;
                    mevcutKullanici.Email = kullanici.Email;
                    mevcutKullanici.AdSoyad = kullanici.AdSoyad;
                    mevcutKullanici.Telefon = kullanici.Telefon;
                    mevcutKullanici.Rol = kullanici.Rol;
                    mevcutKullanici.Aktif = kullanici.Aktif;

                    // Yeni şifre girilmişse şifreyi güncelle
                    if (!string.IsNullOrEmpty(yeniSifre))
                    {
                        mevcutKullanici.Sifre = HashPassword(yeniSifre);
                    }

                    _context.Update(mevcutKullanici);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Kullanıcı başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KullaniciExists(kullanici.KullaniciID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(kullanici);
        }

        private bool KullaniciExists(int id)
        {
            return _context.Kullanicilar.Any(e => e.KullaniciID == id);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
