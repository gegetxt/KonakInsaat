using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminSayiController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AdminSayiController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminSayi
        public async Task<IActionResult> Index()
        {
            var sayilar = await _context.Sayilar
                .OrderBy(s => s.Sira)
                .ToListAsync();
            return View("AdminSayi", sayilar);
        }

        // GET: Admin/AdminSayi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminSayi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sayi sayi)
        {
            if (ModelState.IsValid)
            {
                _context.Sayilar.Add(sayi);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Sayı başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View(sayi);
        }

        // GET: Admin/AdminSayi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sayi = await _context.Sayilar.FindAsync(id);
            if (sayi == null) return NotFound();

            return View(sayi);
        }

        // POST: Admin/AdminSayi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sayi sayi)
        {
            if (id != sayi.SayiID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sayi);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Sayı başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SayiExists(sayi.SayiID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(sayi);
        }

        // GET: Admin/AdminSayi/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sayi = await _context.Sayilar.FindAsync(id);
            if (sayi == null) return NotFound();

            _context.Sayilar.Remove(sayi);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Sayı başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool SayiExists(int id)
        {
            return _context.Sayilar.Any(e => e.SayiID == id);
        }
    }
}
