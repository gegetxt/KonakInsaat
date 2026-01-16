using KonakInsaat.Areas.Admin.Models;
using KonakInsaat.Data;
using KonakInsaat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace KonakInsaat.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly KonakInsaatContext _context;

        public ProjectController(ILogger<ProjectController> logger, IWebHostEnvironment env, KonakInsaatContext context)
        {
            _logger = logger;
            _env = env;
            _context = context;
        }
        // /Project/Details/twin-tower-4/13
        [Route("Project/Details/{folder}/{page}")]
        public IActionResult Details(string folder, int page)
        {
            // Güvenlik kontrolü
            if (string.IsNullOrEmpty(folder) || folder.Contains("..") || folder.Contains("/") || folder.Contains("\\"))
            {
                return NotFound("Geçersiz proje adı.");
            }

            var viewPath = Path.Combine(_env.ContentRootPath, "Views", "Project", "Details", folder, $"{page}.cshtml");

            if (!System.IO.File.Exists(viewPath))
            {
                _logger.LogWarning($"View not found: {viewPath}");
                return NotFound("İstenen proje sayfası bulunamadı.");
            }

            return View($"~/Views/Project/Details/{folder}/{page}.cshtml");
        }
        public async Task<IActionResult> Detail(int id)
        {
            var proje = await _context.Projeler
                .Include(p => p.ProjeResimleri)
                .FirstOrDefaultAsync(p => p.ProjeID == id && p.Aktif);

            if (proje == null)
            {
                return NotFound();
            }

            return View("~/Views/Project/Details/Dynamic.cshtml", proje);
        }

        public async Task<IActionResult> ComingSoon()
        {
            var projeler = await _context.Projeler
                .Where(p => p.Aktif && p.ProjeDurumu == "Yakında")
                .OrderBy(p => p.ProjeAdi)
                .ToListAsync();

            return View("~/Views/Project/ComingSoon.cshtml", projeler);
        }

        public async Task<IActionResult> CompletedProjects()
        {
            var projeler = await _context.Projeler
                .Where(p => p.Aktif && p.ProjeDurumu == "Tamamlandı")
                .OrderBy(p => p.ProjeAdi)
                .ToListAsync();

            return View("~/Views/Project/CompletedProjects.cshtml", projeler);
        }

        public async Task<IActionResult> OngoingProjects()
        {
            var projeler = await _context.Projeler
                .Where(p => p.Aktif && p.ProjeDurumu == "Devam Ediyor")
                .OrderBy(p => p.ProjeAdi)
                .ToListAsync();

            return View("~/Views/Project/OngoingProjects.cshtml", projeler);
        }

        public IActionResult ComingSoon(int id)
        {
            // ContentRootPath kullanarak doğru yolu al
            var viewPath = Path.Combine(_env.ContentRootPath, "Views", "Project", "ComingSoon", $"{id}.cshtml");

            if (!System.IO.File.Exists(viewPath))
            {
                _logger.LogWarning($"View not found: {viewPath}");
                return NotFound("İstenen sayfa bulunamadı.");
            }

            return View($"~/Views/Project/ComingSoon/{id}.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}