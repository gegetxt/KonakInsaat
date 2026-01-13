using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminKurumsalController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminKurumsalController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminKurumsal
        public async Task<IActionResult> Index()
        {
            var kurumsal = await _context.Kurumsal.FirstOrDefaultAsync();
            
            // Eğer kayıt yoksa yeni bir tane oluştur
            if (kurumsal == null)
            {
                kurumsal = new Kurumsal();
                _context.Kurumsal.Add(kurumsal);
                await _context.SaveChangesAsync();
            }

            return View("AdminKurumsal", kurumsal);
        }

        // GET: Admin/AdminKurumsal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var kurumsal = await _context.Kurumsal.FindAsync(id);
            if (kurumsal == null) return NotFound();

            return View(kurumsal);
        }

        // POST: Admin/AdminKurumsal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kurumsal kurumsal)
        {
            if (id != kurumsal.KurumsalID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutKurumsal = await _context.Kurumsal.FindAsync(id);
                    if (mevcutKurumsal == null) return NotFound();

                    // Yeni resim yüklendiyse
                    if (kurumsal.ResimFile != null && kurumsal.ResimFile.Length > 0)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(mevcutKurumsal.HakkimizdaResim))
                        {
                            string eskiResimYolu = Path.Combine(_webHostEnvironment.WebRootPath, mevcutKurumsal.HakkimizdaResim.TrimStart('/'));
                            if (System.IO.File.Exists(eskiResimYolu))
                            {
                                System.IO.File.Delete(eskiResimYolu);
                            }
                        }

                        // Yeni resmi kaydet
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(kurumsal.ResimFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await kurumsal.ResimFile.CopyToAsync(fileStream);
                        }

                        mevcutKurumsal.HakkimizdaResim = "/Resim/" + uniqueFileName;
                    }

                    // Diğer alanları güncelle
                    mevcutKurumsal.HakkimizdaBaslik = kurumsal.HakkimizdaBaslik;
                    mevcutKurumsal.HakkimizdaBaslik_EN = kurumsal.HakkimizdaBaslik_EN;
                    mevcutKurumsal.HakkimizdaBaslik_RU = kurumsal.HakkimizdaBaslik_RU;
                    mevcutKurumsal.HakkimizdaIcerik = kurumsal.HakkimizdaIcerik;
                    mevcutKurumsal.HakkimizdaIcerik_EN = kurumsal.HakkimizdaIcerik_EN;
                    mevcutKurumsal.HakkimizdaIcerik_RU = kurumsal.HakkimizdaIcerik_RU;
                    mevcutKurumsal.Misyon = kurumsal.Misyon;
                    mevcutKurumsal.Misyon_EN = kurumsal.Misyon_EN;
                    mevcutKurumsal.Misyon_RU = kurumsal.Misyon_RU;
                    mevcutKurumsal.Vizyon = kurumsal.Vizyon;
                    mevcutKurumsal.Vizyon_EN = kurumsal.Vizyon_EN;
                    mevcutKurumsal.Vizyon_RU = kurumsal.Vizyon_RU;
                    mevcutKurumsal.VideoUrl = kurumsal.VideoUrl;
                    mevcutKurumsal.GuncellemeTarihi = DateTime.Now;

                    _context.Update(mevcutKurumsal);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Kurumsal bilgiler başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KurumsalExists(kurumsal.KurumsalID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(kurumsal);
        }

        private bool KurumsalExists(int id)
        {
            return _context.Kurumsal.Any(e => e.KurumsalID == id);
        }
    }
}
