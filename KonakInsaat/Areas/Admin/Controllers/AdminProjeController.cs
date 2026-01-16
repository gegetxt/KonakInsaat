using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminProjeController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminProjeController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminProje
        public async Task<IActionResult> Index()
        {
            var projeler = await _context.Projeler
                .OrderByDescending(p => p.OlusturmaTarihi)
                .ToListAsync();
            return View("AdminProje", projeler);
        }

        // GET: Admin/AdminProje/Create
        public IActionResult Create()
        {
            return View("ProjeCreate");
        }

        // POST: Admin/AdminProje/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proje proje)
        {
            if (ModelState.IsValid)
            {
                // Ana resim yükleme
                if (proje.AnaResim != null && proje.AnaResim.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(proje.AnaResim.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await proje.AnaResim.CopyToAsync(fileStream);
                    }

                    proje.AnaResimYolu = "/Resim/" + uniqueFileName;
                }

                _context.Projeler.Add(proje);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Proje başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View("ProjeCreate", proje);
        }

        // GET: Admin/AdminProje/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var proje = await _context.Projeler.FindAsync(id);
            if (proje == null) return NotFound();

            return View(proje);
        }

        // POST: Admin/AdminProje/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proje proje)
        {
            if (id != proje.ProjeID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutProje = await _context.Projeler.FindAsync(id);
                    if (mevcutProje == null) return NotFound();

                    // Yeni ana resim yüklendiyse
                    if (proje.AnaResim != null && proje.AnaResim.Length > 0)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(mevcutProje.AnaResimYolu))
                        {
                            string eskiResimYolu = Path.Combine(_webHostEnvironment.WebRootPath, mevcutProje.AnaResimYolu.TrimStart('/'));
                            if (System.IO.File.Exists(eskiResimYolu))
                            {
                                System.IO.File.Delete(eskiResimYolu);
                            }
                        }

                        // Yeni resmi kaydet
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(proje.AnaResim.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await proje.AnaResim.CopyToAsync(fileStream);
                        }

                        mevcutProje.AnaResimYolu = "/Resim/" + uniqueFileName;
                    }

                    // Diğer alanları güncelle (sadece dolu olanları)
                    if (!string.IsNullOrWhiteSpace(proje.ProjeAdi))
                        mevcutProje.ProjeAdi = proje.ProjeAdi;
                    
                    if (!string.IsNullOrWhiteSpace(proje.ProjeAdi_EN))
                        mevcutProje.ProjeAdi_EN = proje.ProjeAdi_EN;
                    
                    if (!string.IsNullOrWhiteSpace(proje.ProjeAdi_RU))
                        mevcutProje.ProjeAdi_RU = proje.ProjeAdi_RU;
                    
                    mevcutProje.Aciklama = proje.Aciklama;
                    mevcutProje.Aciklama_EN = proje.Aciklama_EN;
                    mevcutProje.Aciklama_RU = proje.Aciklama_RU;
                    mevcutProje.KisaAciklama = proje.KisaAciklama;
                    mevcutProje.KisaAciklama_EN = proje.KisaAciklama_EN;
                    mevcutProje.KisaAciklama_RU = proje.KisaAciklama_RU;
                    
                    if (!string.IsNullOrWhiteSpace(proje.ProjeDurumu))
                        mevcutProje.ProjeDurumu = proje.ProjeDurumu;
                    
                    mevcutProje.Konum = proje.Konum;
                    mevcutProje.Metrekare = proje.Metrekare;
                    mevcutProje.OdaSayisi = proje.OdaSayisi;
                    mevcutProje.TeslimTarihi = proje.TeslimTarihi;
                    mevcutProje.ProjeBaslangicTarihi = proje.ProjeBaslangicTarihi;
                    mevcutProje.ProjeTamamlanmaTarihi = proje.ProjeTamamlanmaTarihi;
                    mevcutProje.BuildYear = proje.BuildYear;
                    mevcutProje.Adres = proje.Adres;
                    mevcutProje.OrtakAlanlar = proje.OrtakAlanlar;
                    mevcutProje.DaireTipleriABlok = proje.DaireTipleriABlok;
                    mevcutProje.DaireTipleriBBlok = proje.DaireTipleriBBlok;
                    mevcutProje.Telefon1 = proje.Telefon1;
                    mevcutProje.Telefon1Diller = proje.Telefon1Diller;
                    mevcutProje.Telefon2 = proje.Telefon2;
                    mevcutProje.Telefon2Diller = proje.Telefon2Diller;
                    mevcutProje.GoogleMapsUrl = proje.GoogleMapsUrl;
                    mevcutProje.Aktif = proje.Aktif;

                    _context.Update(mevcutProje);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Proje başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjeExists(proje.ProjeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(proje);
        }

        // GET: Admin/AdminProje/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var proje = await _context.Projeler
                .Include(p => p.ProjeResimleri)
                .FirstOrDefaultAsync(p => p.ProjeID == id);
            
            if (proje == null) return NotFound();

            // Ana resmi sil
            if (!string.IsNullOrEmpty(proje.AnaResimYolu))
            {
                string resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, proje.AnaResimYolu.TrimStart('/'));
                if (System.IO.File.Exists(resimYolu))
                {
                    System.IO.File.Delete(resimYolu);
                }
            }

            // Detay resimlerini sil
            if (proje.ProjeResimleri != null && proje.ProjeResimleri.Any())
            {
                foreach (var resim in proje.ProjeResimleri)
                {
                    if (!string.IsNullOrEmpty(resim.ResimYolu))
                    {
                        string resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, resim.ResimYolu.TrimStart('/'));
                        if (System.IO.File.Exists(resimYolu))
                        {
                            System.IO.File.Delete(resimYolu);
                        }
                    }
                }
            }

            _context.Projeler.Remove(proje);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Proje başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/AdminProje/DetayResimGetir/5
        public async Task<IActionResult> DetayResimGetir(int id)
        {
            var proje = await _context.Projeler
                .Include(p => p.ProjeResimleri)
                .FirstOrDefaultAsync(p => p.ProjeID == id);

            if (proje == null) return NotFound();

            ViewBag.ProjeID = id;
            ViewBag.ProjeAdi = proje.ProjeAdi;

            return View(proje.ProjeResimleri?.OrderBy(r => r.Sira).ToList() ?? new List<ProjeResim>());
        }

        // POST: Admin/AdminProje/ResimEkle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResimEkle(int projeID, IFormFile resimDosyasi, string? baslik, int sira)
        {
            if (resimDosyasi != null && resimDosyasi.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(resimDosyasi.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await resimDosyasi.CopyToAsync(fileStream);
                }

                var projeResim = new ProjeResim
                {
                    ProjeID = projeID,
                    ResimYolu = "/Resim/" + uniqueFileName,
                    Baslik = baslik,
                    Sira = sira
                };

                _context.ProjeResimleri.Add(projeResim);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Resim başarıyla eklendi!";
            }

            return RedirectToAction("DetayResimGetir", new { id = projeID });
        }

        // GET: Admin/AdminProje/ResimSil/5
        [HttpGet]
        public async Task<IActionResult> ResimSil(int id)
        {
            var projeResim = await _context.ProjeResimleri.FindAsync(id);
            if (projeResim == null) return NotFound();

            var projeID = projeResim.ProjeID;

            // Resmi sil
            if (!string.IsNullOrEmpty(projeResim.ResimYolu))
            {
                string resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, projeResim.ResimYolu.TrimStart('/'));
                if (System.IO.File.Exists(resimYolu))
                {
                    System.IO.File.Delete(resimYolu);
                }
            }

            _context.ProjeResimleri.Remove(projeResim);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Resim başarıyla silindi!";
            return RedirectToAction("DetayResimGetir", new { id = projeID });
        }

        private bool ProjeExists(int id)
        {
            return _context.Projeler.Any(e => e.ProjeID == id);
        }
    }
}
