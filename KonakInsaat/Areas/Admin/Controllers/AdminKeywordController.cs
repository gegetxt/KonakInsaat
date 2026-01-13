using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminKeywordController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AdminKeywordController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminKeyword
        public async Task<IActionResult> Index()
        {
            var keywords = await _context.Keywords.ToListAsync();
            return View("AdminKeyword", keywords);
        }

        // GET: Admin/AdminKeyword/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminKeyword/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Keyword keyword)
        {
            if (ModelState.IsValid)
            {
                _context.Keywords.Add(keyword);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Anahtar kelime başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View(keyword);
        }

        // GET: Admin/AdminKeyword/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var keyword = await _context.Keywords.FindAsync(id);
            if (keyword == null) return NotFound();

            return View(keyword);
        }

        // POST: Admin/AdminKeyword/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Keyword keyword)
        {
            if (id != keyword.KeywordID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keyword);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Anahtar kelime başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeywordExists(keyword.KeywordID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(keyword);
        }

        // GET: Admin/AdminKeyword/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var keyword = await _context.Keywords.FindAsync(id);
            if (keyword == null) return NotFound();

            _context.Keywords.Remove(keyword);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Anahtar kelime başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool KeywordExists(int id)
        {
            return _context.Keywords.Any(e => e.KeywordID == id);
        }
    }
}
