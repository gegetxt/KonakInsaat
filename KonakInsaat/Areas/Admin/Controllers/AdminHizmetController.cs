using KonakInsaat.Areas.Admin.Models;
using KonakInsaat.Data;
using KonakInsaat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminHizmetController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminHizmetController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminHizmet
        public async Task<IActionResult> Index()
        {
            var hizmetler = await _context.Hizmetler
                .OrderByDescending(h => h.OlusturmaTarihi)
                .ToListAsync();
            return View("AdminHizmet", hizmetler);
        }

        // GET: Admin/AdminHizmet/HizmetCreate
        public IActionResult HizmetCreate()
        {
            return View();
        }

        // POST: Admin/AdminHizmet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                // Resim yükleme işlemi
                if (hizmet.Resim != null && hizmet.Resim.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(hizmet.Resim.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await hizmet.Resim.CopyToAsync(fileStream);
                    }

                    hizmet.ResimYolu = uniqueFileName;
                }

                _context.Hizmetler.Add(hizmet);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Hizmet başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View("HizmetCreate", hizmet);
        }

        // GET: Admin/AdminHizmet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet == null)
            {
                return NotFound();
            }

            return View(hizmet);
        }

        // POST: Admin/AdminHizmet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hizmet hizmet)
        {
            if (id != hizmet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutHizmet = await _context.Hizmetler.FindAsync(id);
                    if (mevcutHizmet == null)
                    {
                        return NotFound();
                    }

                    // Yeni resim yüklendiyse
                    if (hizmet.Resim != null && hizmet.Resim.Length > 0)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(mevcutHizmet.ResimYolu))
                        {
                            string eskiResimYolu = Path.Combine(_webHostEnvironment.WebRootPath, "Resim", mevcutHizmet.ResimYolu);
                            if (System.IO.File.Exists(eskiResimYolu))
                            {
                                System.IO.File.Delete(eskiResimYolu);
                            }
                        }

                        // Yeni resmi kaydet
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(hizmet.Resim.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await hizmet.Resim.CopyToAsync(fileStream);
                        }

                        mevcutHizmet.ResimYolu = uniqueFileName;
                    }

                    // Diğer alanları güncelle
                    mevcutHizmet.Baslik = hizmet.Baslik;
                    mevcutHizmet.Baslik_EN = hizmet.Baslik_EN;
                    mevcutHizmet.Baslik_RU = hizmet.Baslik_RU;
                    mevcutHizmet.Aciklama = hizmet.Aciklama;
                    mevcutHizmet.Aciklama_EN = hizmet.Aciklama_EN;
                    mevcutHizmet.Aciklama_RU = hizmet.Aciklama_RU;
                    mevcutHizmet.KisaAciklama = hizmet.KisaAciklama;
                    mevcutHizmet.KisaAciklama_EN = hizmet.KisaAciklama_EN;
                    mevcutHizmet.KisaAciklama_RU = hizmet.KisaAciklama_RU;

                    _context.Update(mevcutHizmet);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Hizmet başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HizmetExists(hizmet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(hizmet);
        }

        // GET: Admin/AdminHizmet/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet == null)
            {
                return NotFound();
            }

            // Resmi sil
            if (!string.IsNullOrEmpty(hizmet.ResimYolu))
            {
                string resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, "Resim", hizmet.ResimYolu);
                if (System.IO.File.Exists(resimYolu))
                {
                    System.IO.File.Delete(resimYolu);
                }
            }

            _context.Hizmetler.Remove(hizmet);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Hizmet başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool HizmetExists(int id)
        {
            return _context.Hizmetler.Any(e => e.Id == id);
        }
    }
}
