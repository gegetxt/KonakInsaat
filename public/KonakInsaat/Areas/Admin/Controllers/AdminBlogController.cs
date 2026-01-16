using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class AdminBlogController : Controller
    {
        private readonly KonakInsaatContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminBlogController(KonakInsaatContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminBlog
        public async Task<IActionResult> Index()
        {
            var bloglar = await _context.Bloglar
                .OrderByDescending(b => b.YayinTarihi)
                .ToListAsync();
            return View("AdminBlog", bloglar);
        }

        // GET: Admin/AdminBlog/BlogCreate
        public IActionResult BlogCreate()
        {
            return View();
        }

        // POST: Admin/AdminBlog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (ModelState.IsValid)
            {
                // Resim yükleme
                if (blog.Resim != null && blog.Resim.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(blog.Resim.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await blog.Resim.CopyToAsync(fileStream);
                    }

                    blog.ResimYolu = uniqueFileName;
                }

                _context.Bloglar.Add(blog);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Blog başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }

            return View("BlogCreate", blog);
        }

        // GET: Admin/AdminBlog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _context.Bloglar.FindAsync(id);
            if (blog == null) return NotFound();

            return View(blog);
        }

        // POST: Admin/AdminBlog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog)
        {
            if (id != blog.BlogID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutBlog = await _context.Bloglar.FindAsync(id);
                    if (mevcutBlog == null) return NotFound();

                    // Yeni resim yüklendiyse
                    if (blog.Resim != null && blog.Resim.Length > 0)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(mevcutBlog.ResimYolu))
                        {
                            string eskiResimYolu = Path.Combine(_webHostEnvironment.WebRootPath, "Resim", mevcutBlog.ResimYolu);
                            if (System.IO.File.Exists(eskiResimYolu))
                            {
                                System.IO.File.Delete(eskiResimYolu);
                            }
                        }

                        // Yeni resmi kaydet
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Resim");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(blog.Resim.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await blog.Resim.CopyToAsync(fileStream);
                        }

                        mevcutBlog.ResimYolu = uniqueFileName;
                    }

                    // Diğer alanları güncelle
                    mevcutBlog.Baslik = blog.Baslik;
                    mevcutBlog.Baslik_EN = blog.Baslik_EN;
                    mevcutBlog.Baslik_RU = blog.Baslik_RU;
                    mevcutBlog.Icerik = blog.Icerik;
                    mevcutBlog.Icerik_EN = blog.Icerik_EN;
                    mevcutBlog.Icerik_RU = blog.Icerik_RU;
                    mevcutBlog.KisaAciklama = blog.KisaAciklama;
                    mevcutBlog.KisaAciklama_EN = blog.KisaAciklama_EN;
                    mevcutBlog.KisaAciklama_RU = blog.KisaAciklama_RU;
                    mevcutBlog.Aktif = blog.Aktif;

                    _context.Update(mevcutBlog);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Blog başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(blog);
        }

        // GET: Admin/AdminBlog/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _context.Bloglar.FindAsync(id);
            if (blog == null) return NotFound();

            // Resmi sil
            if (!string.IsNullOrEmpty(blog.ResimYolu))
            {
                string resimYolu = Path.Combine(_webHostEnvironment.WebRootPath, "Resim", blog.ResimYolu);
                if (System.IO.File.Exists(resimYolu))
                {
                    System.IO.File.Delete(resimYolu);
                }
            }

            _context.Bloglar.Remove(blog);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Blog başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Bloglar.Any(e => e.BlogID == id);
        }
    }
}
