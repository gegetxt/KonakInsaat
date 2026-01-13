using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminSosyalController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AdminSosyalController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminSosyal
        public async Task<IActionResult> Index()
        {
            var sosyalMedya = await _context.SosyalMedya
                .OrderBy(s => s.Sira)
                .ToListAsync();
            return View("AdminSosyal", sosyalMedya);
        }

        // GET: Admin/AdminSosyal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminSosyal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sosyal sosyal)
        {
            if (ModelState.IsValid)
            {
                _context.SosyalMedya.Add(sosyal);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Sosyal medya hesabı başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View(sosyal);
        }

        // GET: Admin/AdminSosyal/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sosyal = await _context.SosyalMedya.FindAsync(id);
            if (sosyal == null) return NotFound();

            return View(sosyal);
        }

        // POST: Admin/AdminSosyal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sosyal sosyal)
        {
            if (id != sosyal.SosyalID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sosyal);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Sosyal medya hesabı başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SosyalExists(sosyal.SosyalID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(sosyal);
        }

        // GET: Admin/AdminSosyal/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var sosyal = await _context.SosyalMedya.FindAsync(id);
            if (sosyal == null) return NotFound();

            _context.SosyalMedya.Remove(sosyal);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Sosyal medya hesabı başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool SosyalExists(int id)
        {
            return _context.SosyalMedya.Any(e => e.SosyalID == id);
        }
    }
}
