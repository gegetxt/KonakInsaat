using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // İstersen daha sonra [Authorize] yaparız
    public class AdminOfficeController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AdminOfficeController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminOffice
        public async Task<IActionResult> Index()
        {
            var ofisler = await _context.OfficeLocations
                .OrderBy(o => o.Sira)
                .ToListAsync();

            return View("AdminOffice", ofisler);
        }

        // GET: Admin/AdminOffice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ofis = await _context.OfficeLocations.FindAsync(id);
            if (ofis == null) return NotFound();

            return View(ofis);
        }

        // POST: Admin/AdminOffice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OfficeLocation ofis)
        {
            if (id != ofis.OfficeLocationID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ofis);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Ofis bilgileri başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.OfficeLocations.Any(e => e.OfficeLocationID == id))
                        return NotFound();
                    throw;
                }
            }

            return View(ofis);
        }
    }
}


