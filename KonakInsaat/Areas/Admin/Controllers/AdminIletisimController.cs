using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminIletisimController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminIletisimController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminIletisim
        public async Task<IActionResult> Index()
        {
            var list = await _context.Iletisim.ToListAsync();
            return View("AdminIletisim", list);
        }

        // GET: Admin/AdminIletisim/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iletisim = await _context.Iletisim.FindAsync(id);
            if (iletisim == null)
            {
                return NotFound();
            }
            return View(iletisim);
        }

        // POST: Admin/AdminIletisim/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Iletisim iletisim)
        {
            if (id != iletisim.IletisimID)
            {
                return NotFound();
            }

            // Mevcut kaydı veritabanından çek (her durumda)
            var mevcutKayit = await _context.Iletisim.FindAsync(id);
            if (mevcutKayit == null)
            {
                return NotFound();
            }

            try
            {
                // Form'dan gelen değerleri mevcut kayda aktar
                // ÖNEMLİ: Sadece gerçekten değişen değerleri güncelle
                // Eğer form'dan gelen değer null veya boş string ise, mevcut değeri koru
                
                // Baslik kontrolü - sadece değişmişse güncelle
                if (iletisim.Baslik != null && iletisim.Baslik.Trim() != mevcutKayit.Baslik?.Trim())
                {
                    if (!string.IsNullOrWhiteSpace(iletisim.Baslik))
                        mevcutKayit.Baslik = iletisim.Baslik.Trim();
                }
                
                // Adres kontrolü
                if (iletisim.Adres_1 != null && iletisim.Adres_1.Trim() != mevcutKayit.Adres_1?.Trim())
                {
                    if (!string.IsNullOrWhiteSpace(iletisim.Adres_1))
                        mevcutKayit.Adres_1 = iletisim.Adres_1.Trim();
                }
                
                // Telefon kontrolü
                if (iletisim.Telefon != null && iletisim.Telefon.Trim() != mevcutKayit.Telefon?.Trim())
                {
                    if (!string.IsNullOrWhiteSpace(iletisim.Telefon))
                        mevcutKayit.Telefon = iletisim.Telefon.Trim();
                }
                
                // GSM kontrolü
                if (iletisim.Gsm != null && iletisim.Gsm.Trim() != mevcutKayit.Gsm?.Trim())
                {
                    if (!string.IsNullOrWhiteSpace(iletisim.Gsm))
                        mevcutKayit.Gsm = iletisim.Gsm.Trim();
                }
                
                // WhatsApp kontrolü
                if (iletisim.Whatsapp != null && iletisim.Whatsapp.Trim() != mevcutKayit.Whatsapp?.Trim())
                {
                    if (!string.IsNullOrWhiteSpace(iletisim.Whatsapp))
                        mevcutKayit.Whatsapp = iletisim.Whatsapp.Trim();
                }
                
                // Mail kontrolü
                if (iletisim.Mail != null && iletisim.Mail.Trim() != mevcutKayit.Mail?.Trim())
                {
                    if (!string.IsNullOrWhiteSpace(iletisim.Mail))
                        mevcutKayit.Mail = iletisim.Mail.Trim();
                }
                
                // Harita - her zaman güncelle (null da olabilir)
                mevcutKayit.Harita = iletisim.Harita?.Trim();

                // Resim yükleme işlemi
                if (iletisim.ResimFile != null && iletisim.ResimFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Eski resmi sil
                    if (!string.IsNullOrEmpty(mevcutKayit.Resim))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, mevcutKayit.Resim.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Yeni resmi kaydet
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + iletisim.ResimFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await iletisim.ResimFile.CopyToAsync(fileStream);
                    }

                    mevcutKayit.Resim = "/Resim/" + uniqueFileName;
                }

                _context.Update(mevcutKayit);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "İletişim bilgileri başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IletisimExists(iletisim.IletisimID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mevcut kaydı döndür
                ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                return View(mevcutKayit);
            }
        }

        private bool IletisimExists(int id)
        {
            return _context.Iletisim.Any(e => e.IletisimID == id);
        }
    }
}