using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminGaleriController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminGaleriController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminGaleri
        public async Task<IActionResult> Index()
        {
            var galeriler = await _context.Galeriler
                .OrderBy(g => g.Sira)
                .ToListAsync();
            return View("AdminGaleri", galeriler);
        }

        // GET: Admin/AdminGaleri/Create
        public IActionResult Create()
        {
            return View("GaleriCreate");
        }

        // POST: Admin/AdminGaleri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Galeri galeri)
        {
            if (ModelState.IsValid)
            {
                // Resim yükleme
                if (galeri.Resim != null && galeri.Resim.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(galeri.Resim.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await galeri.Resim.CopyToAsync(fileStream);
                    }

                    galeri.ResimYolu = "/Resim/" + uniqueFileName;
                }

                _context.Galeriler.Add(galeri);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Galeri resmi başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View("GaleriCreate", galeri);
        }

        // GET: Admin/AdminGaleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var galeri = await _context.Galeriler.FindAsync(id);
            if (galeri == null) return NotFound();

            return View(galeri);
        }

        // POST: Admin/AdminGaleri/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Galeri galeri)
        {
            if (id != galeri.GaleriID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutGaleri = await _context.Galeriler.FindAsync(id);
                    if (mevcutGaleri == null) return NotFound();

                    // Yeni resim yüklendiyse
                    if (galeri.Resim != null && galeri.Resim.Length > 0)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(mevcutGaleri.ResimYolu))
                        {
                            string eskiResimYolu = Path.Combine(_webHostEnvironment.WebRootPath, mevcutGaleri.ResimYolu.TrimStart('/'));
                            if (System.IO.File.Exists(eskiResimYolu))
                            {
                                System.IO.File.Delete(eskiResimYolu);
                            }
                        }

                        // Yeni resmi kaydet
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(galeri.Resim.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await galeri.Resim.CopyToAsync(fileStream);
                        }

                        mevcutGaleri.ResimYolu = "/Resim/" + uniqueFileName;
                    }

                    // Diğer alanları güncelle
                    mevcutGaleri.Baslik = galeri.Baslik;
                    mevcutGaleri.Baslik_EN = galeri.Baslik_EN;
                    mevcutGaleri.Baslik_RU = galeri.Baslik_RU;
                    mevcutGaleri.Kategori = galeri.Kategori;
                    mevcutGaleri.Sira = galeri.Sira;
                    mevcutGaleri.Aktif = galeri.Aktif;

                    _context.Update(mevcutGaleri);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Galeri başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GaleriExists(galeri.GaleriID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(galeri);
        }

        // GET: Admin/AdminGaleri/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var galeri = await _context.Galeriler.FindAsync(id);
            if (galeri == null) return NotFound();

            // Resmi sil
            if (!string.IsNullOrEmpty(galeri.ResimYolu))
            {
                string resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, galeri.ResimYolu.TrimStart('/'));
                if (System.IO.File.Exists(resimYolu))
                {
                    System.IO.File.Delete(resimYolu);
                }
            }

            _context.Galeriler.Remove(galeri);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Galeri başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool GaleriExists(int id)
        {
            return _context.Galeriler.Any(e => e.GaleriID == id);
        }
    }
}
