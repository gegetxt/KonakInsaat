using KonakInsaat.Areas.Admin.Models;
using KonakInsaat.Data;
using KonakInsaat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KonakInsaat.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ILogger<ServiceController> _logger;
        private readonly KonakInsaatContext _context;

        public ServiceController(ILogger<ServiceController> logger, KonakInsaatContext context)
        {
            _logger = logger;
            _context = context;
        }

        // /Service/Details/3 => Veritabanından hizmeti çek
        public async Task<IActionResult> Details(int id)
        {
            var hizmet = await _context.Hizmetler.FindAsync(id);
            
            // Eğer ID ile bulunamazsa ve ID=3 ise, "Kentsel Dönüşüm" başlıklı hizmeti ara
            if (hizmet == null && id == 3)
            {
                hizmet = await _context.Hizmetler
                    .FirstOrDefaultAsync(h => h.Baslik != null && h.Baslik.ToLower().Contains("kentsel"));
            }
            
            if (hizmet == null)
            {
                return NotFound("İstenen hizmet bulunamadı.");
            }

            return View("~/Views/Service/Details/Dynamic.cshtml", hizmet);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

