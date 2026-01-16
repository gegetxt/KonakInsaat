using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminKatalogController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminKatalogController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminKatalog
        public async Task<IActionResult> Index()
        {
            var kataloglar = await _context.Kataloglar.ToListAsync();
            
            // Eğer kayıt yoksa yeni bir tane oluştur
            if (!kataloglar.Any())
            {
                var yeniKatalog = new Katalog
                {
                    Baslik = "PDF Katalogu"
                };
                _context.Kataloglar.Add(yeniKatalog);
                await _context.SaveChangesAsync();
                kataloglar = await _context.Kataloglar.ToListAsync();
            }

            return View("AdminKatalog", kataloglar);
        }

        // GET: Admin/AdminKatalog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var katalog = await _context.Kataloglar.FindAsync(id);
            if (katalog == null) return NotFound();

            return View(katalog);
        }

        // POST: Admin/AdminKatalog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Katalog model, IFormFile? PdfDosya)
        {
            if (id != model.KatalogID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutKatalog = await _context.Kataloglar.FindAsync(id);
                    if (mevcutKatalog == null) return NotFound();

                    // Eğer yeni PDF dosyası yüklendiyse
                    if (PdfDosya != null && PdfDosya.Length > 0)
                    {
                        // Dosya uzantısı kontrolü
                        var extension = Path.GetExtension(PdfDosya.FileName).ToLower();
                        if (extension != ".pdf")
                        {
                            ModelState.AddModelError("PdfDosya", "Sadece PDF dosyası yükleyebilirsiniz.");
                            return View(model);
                        }

                        // Eski dosyayı sil (varsa)
                        if (!string.IsNullOrEmpty(mevcutKatalog.Link))
                        {
                            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, mevcutKatalog.Link.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Yeni dosya adı oluştur (benzersiz)
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(PdfDosya.FileName)}";
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "katalog");

                        // Klasör yoksa oluştur
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var filePath = Path.Combine(uploadPath, fileName);

                        // Dosyayı kaydet
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PdfDosya.CopyToAsync(stream);
                        }

                        mevcutKatalog.Link = $"/uploads/katalog/{fileName}";
                    }

                    // Diğer alanları güncelle
                    mevcutKatalog.Baslik = model.Baslik;

                    _context.Update(mevcutKatalog);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Katalog başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Güncelleme sırasında bir hata oluştu: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Admin/AdminKatalog/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var katalog = await _context.Kataloglar.FindAsync(id);
            if (katalog == null) return NotFound();

            // PDF dosyasını sil
            if (!string.IsNullOrEmpty(katalog.Link))
            {
                string dosyaYolu = Path.Combine(_webHostEnvironment.WebRootPath, katalog.Link.TrimStart('/'));
                if (System.IO.File.Exists(dosyaYolu))
                {
                    System.IO.File.Delete(dosyaYolu);
                }
            }

            _context.Kataloglar.Remove(katalog);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Katalog başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }
    }
}