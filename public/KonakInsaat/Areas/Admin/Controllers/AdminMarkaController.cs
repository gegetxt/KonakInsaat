using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminMarkaController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminMarkaController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminMarka
        public async Task<IActionResult> Index()
        {
            var markalar = await _context.Markalar
                .OrderBy(m => m.Sira)
                .ToListAsync();
            return View("AdminMarka", markalar);
        }

        // GET: Admin/AdminMarka/Create
        public IActionResult Create()
        {
            return View("MarkaCreate");
        }

        // POST: Admin/AdminMarka/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Marka marka)
        {
            if (ModelState.IsValid)
            {
                // Resim yükleme
                if (marka.Resim != null && marka.Resim.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(marka.Resim.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await marka.Resim.CopyToAsync(fileStream);
                    }

                    marka.ResimYolu = "/Resim/" + uniqueFileName;
                }

                _context.Markalar.Add(marka);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Referans başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View("MarkaCreate", marka);
        }

        // GET: Admin/AdminMarka/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var marka = await _context.Markalar.FindAsync(id);
            if (marka == null) return NotFound();

            return View(marka);
        }

        // POST: Admin/AdminMarka/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Marka marka)
        {
            if (id != marka.MarkaID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutMarka = await _context.Markalar.FindAsync(id);
                    if (mevcutMarka == null) return NotFound();

                    // Yeni resim yüklendiyse
                    if (marka.Resim != null && marka.Resim.Length > 0)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(mevcutMarka.ResimYolu))
                        {
                            string eskiResimYolu = Path.Combine(_webHostEnvironment.WebRootPath, mevcutMarka.ResimYolu.TrimStart('/'));
                            if (System.IO.File.Exists(eskiResimYolu))
                            {
                                System.IO.File.Delete(eskiResimYolu);
                            }
                        }

                        // Yeni resmi kaydet
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(marka.Resim.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await marka.Resim.CopyToAsync(fileStream);
                        }

                        mevcutMarka.ResimYolu = "/Resim/" + uniqueFileName;
                    }

                    // Diğer alanları güncelle
                    mevcutMarka.MarkaAdi = marka.MarkaAdi;
                    mevcutMarka.Link = marka.Link;
                    mevcutMarka.Sira = marka.Sira;
                    mevcutMarka.Aktif = marka.Aktif;

                    _context.Update(mevcutMarka);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Referans başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkaExists(marka.MarkaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(marka);
        }

        // GET: Admin/AdminMarka/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var marka = await _context.Markalar.FindAsync(id);
            if (marka == null) return NotFound();

            // Resmi sil
            if (!string.IsNullOrEmpty(marka.ResimYolu))
            {
                string resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, marka.ResimYolu.TrimStart('/'));
                if (System.IO.File.Exists(resimYolu))
                {
                    System.IO.File.Delete(resimYolu);
                }
            }

            _context.Markalar.Remove(marka);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Referans başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool MarkaExists(int id)
        {
            return _context.Markalar.Any(e => e.MarkaID == id);
        }
    }
}
