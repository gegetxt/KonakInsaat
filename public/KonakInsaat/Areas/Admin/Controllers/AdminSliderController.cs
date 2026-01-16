using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminSliderController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminSliderController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminSlider
        public async Task<IActionResult> Index()
        {
            var sliderlar = await _context.Sliderlar
                .OrderBy(s => s.Sira)
                .ToListAsync();
            return View("AdminSlider", sliderlar);
        }

        // GET: Admin/AdminSlider/Create
        public IActionResult Create()
        {
            return View("SliderCreate");
        }

        // POST: Admin/AdminSlider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                // Resim yükleme
                if (slider.Resim != null && slider.Resim.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(slider.Resim.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await slider.Resim.CopyToAsync(fileStream);
                    }

                    slider.ResimYolu = "/Resim/" + uniqueFileName;
                }

                _context.Sliderlar.Add(slider);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Slider başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View("SliderCreate", slider);
        }

        // GET: Admin/AdminSlider/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var slider = await _context.Sliderlar.FindAsync(id);
            if (slider == null) return NotFound();

            return View(slider);
        }

        // POST: Admin/AdminSlider/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider)
        {
            if (id != slider.SliderID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutSlider = await _context.Sliderlar.FindAsync(id);
                    if (mevcutSlider == null) return NotFound();

                    // Yeni resim yüklendiyse
                    if (slider.Resim != null && slider.Resim.Length > 0)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(mevcutSlider.ResimYolu))
                        {
                            string eskiResimYolu = Path.Combine(_webHostEnvironment.WebRootPath, mevcutSlider.ResimYolu.TrimStart('/'));
                            if (System.IO.File.Exists(eskiResimYolu))
                            {
                                System.IO.File.Delete(eskiResimYolu);
                            }
                        }

                        // Yeni resmi kaydet
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(slider.Resim.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await slider.Resim.CopyToAsync(fileStream);
                        }

                        mevcutSlider.ResimYolu = "/Resim/" + uniqueFileName;
                    }

                    // Diğer alanları güncelle
                    mevcutSlider.Baslik = slider.Baslik;
                    mevcutSlider.Baslik_EN = slider.Baslik_EN;
                    mevcutSlider.Baslik_RU = slider.Baslik_RU;
                    mevcutSlider.AltBaslik = slider.AltBaslik;
                    mevcutSlider.AltBaslik_EN = slider.AltBaslik_EN;
                    mevcutSlider.AltBaslik_RU = slider.AltBaslik_RU;
                    mevcutSlider.Sira = slider.Sira;
                    mevcutSlider.Aktif = slider.Aktif;

                    _context.Update(mevcutSlider);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Slider başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.SliderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(slider);
        }

        // GET: Admin/AdminSlider/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slider = await _context.Sliderlar.FindAsync(id);
            if (slider == null) return NotFound();

            // Resmi sil
            if (!string.IsNullOrEmpty(slider.ResimYolu))
            {
                string resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, slider.ResimYolu.TrimStart('/'));
                if (System.IO.File.Exists(resimYolu))
                {
                    System.IO.File.Delete(resimYolu);
                }
            }

            _context.Sliderlar.Remove(slider);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Slider başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
            return _context.Sliderlar.Any(e => e.SliderID == id);
        }
    }
}
