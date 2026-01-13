using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminMesajController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AdminMesajController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminMesaj
        public async Task<IActionResult> Index()
        {
            var mesajlar = await _context.Mesajlar
                .OrderByDescending(m => m.GonderimTarihi)
                .ToListAsync();
            return View("AdminMesaj", mesajlar);
        }

        // GET: Admin/AdmnMesaj/Detay/5
        public async Task<IActionResult> Detay(int? id)
        {
            if (id == null) return NotFound();

            var mesaj = await _context.Mesajlar.FindAsync(id);
            if (mesaj == null) return NotFound();

            // Mesajı okundu olarak işaretle
            if (!mesaj.Okundu)
            {
                mesaj.Okundu = true;
                _context.Update(mesaj);
                await _context.SaveChangesAsync();
            }

            return View(mesaj);
        }

        // GET: Admin/AdmnMesaj/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var mesaj = await _context.Mesajlar.FindAsync(id);
            if (mesaj == null) return NotFound();

            _context.Mesajlar.Remove(mesaj);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Mesaj başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/AdmnMesaj/OkunduIsaretle/5
        [HttpPost]
        public async Task<IActionResult> OkunduIsaretle(int id)
        {
            var mesaj = await _context.Mesajlar.FindAsync(id);
            if (mesaj != null)
            {
                mesaj.Okundu = true;
                _context.Update(mesaj);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        // POST: Admin/AdmnMesaj/YanitlandiIsaretle/5
        [HttpPost]
        public async Task<IActionResult> YanitlandiIsaretle(int id)
        {
            var mesaj = await _context.Mesajlar.FindAsync(id);
            if (mesaj != null)
            {
                mesaj.Yanitlandi = true;
                _context.Update(mesaj);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }
    }
}
