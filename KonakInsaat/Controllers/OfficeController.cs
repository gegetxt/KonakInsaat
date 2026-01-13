using KonakInsaat.Models;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;

namespace KonakInsaat.Controllers
{
    public class OfficeController : Controller
    {
        private readonly ILogger<OfficeController> _logger;
        private readonly KonakInsaatContext _context;

        public OfficeController(ILogger<OfficeController> logger, KonakInsaatContext context)
        {
            _logger = logger;
            _context = context;
        }

        // /Office/Details/1 => Satış Ofisi
        // /Office/Details/3 => Merkez Ofis
        public async Task<IActionResult> Details(int id)
        {
            OfficeLocation? ofis = null;

            // Mevcut URL yapısını bozmadan id'ye göre tip eşle
            if (id == 1)
            {
                ofis = await _context.OfficeLocations
                    .Where(o => o.Aktif && o.Tip != null && o.Tip.ToLower().Contains("satış"))
                    .OrderBy(o => o.Sira)
                    .FirstOrDefaultAsync();
            }
            else if (id == 3)
            {
                ofis = await _context.OfficeLocations
                    .Where(o => o.Aktif && o.Tip != null && o.Tip.ToLower().Contains("merkez"))
                    .OrderBy(o => o.Sira)
                    .FirstOrDefaultAsync();
            }

            // Tip'e göre bulunamazsa doğrudan ID ile dene
            ofis ??= await _context.OfficeLocations
                .FirstOrDefaultAsync(o => o.OfficeLocationID == id && o.Aktif);

            if (ofis != null)
            {
                // 1.cshtml => Satış Ofisi, 3.cshtml => Merkez Ofis tasarımını kullanmaya devam et
                var viewPath = $"~/Views/Office/Details/{id}.cshtml";
                if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Office", "Details", $"{id}.cshtml")))
                {
                    return View(viewPath, ofis);
                }

                // Eğer özel view yoksa ortak bir default ofis view'i kullan
                return View("~/Views/Office/Details/Default.cshtml", ofis);
            }

            // Hiç ofis yoksa eski davranışı koru
            var fallbackViewPath = $"~/Views/Office/Details/{id}.cshtml";
            if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Office", "Details", $"{id}.cshtml")))
            {
                return View(fallbackViewPath);
            }

            return View("~/Views/Office/Details/Default.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
