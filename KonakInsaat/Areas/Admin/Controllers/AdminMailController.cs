using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminMailController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AdminMailController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminMail
        public async Task<IActionResult> Index()
        {
            var mailAyarlari = await _context.MailAyarlari.ToListAsync();
            return View("AdminMail", mailAyarlari);
        }

        // GET: Admin/AdminMail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var mail = await _context.MailAyarlari.FindAsync(id);
            if (mail == null) return NotFound();

            return View(mail);
        }

        // POST: Admin/AdminMail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mail mail)
        {
            if (id != mail.MailID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mail);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Mail ayarları başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailExists(mail.MailID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(mail);
        }

        private bool MailExists(int id)
        {
            return _context.MailAyarlari.Any(e => e.MailID == id);
        }
    }
}
