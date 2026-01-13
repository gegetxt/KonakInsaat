using System.Diagnostics;
using KonakInsaat.Models;
using KonakInsaat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KonakInsaat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KonakInsaatContext _context;

        public HomeController(ILogger<HomeController> logger, KonakInsaatContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliderlar
                .Where(s => s.Aktif)
                .OrderBy(s => s.Sira)
                .ToListAsync();
            
            var projeler = await _context.Projeler
                .Where(p => p.Aktif)
                .OrderByDescending(p => p.OlusturmaTarihi)
                .Take(10)
                .ToListAsync();
            
            ViewBag.Projeler = projeler;
            
            return View(sliders);
        }

        public async Task<IActionResult> About()
        {
            var kurumsal = await _context.Kurumsal.FirstOrDefaultAsync();
            return View(kurumsal);
        }

        public async Task<IActionResult> Contact()
        {
            var iletisimler = await _context.Iletisim
                .OrderBy(i => i.Baslik)
                .ToListAsync();

            return View(iletisimler);
        }

        public async Task<IActionResult> Team()
        {
            var ekip = await _context.TeamMembers
                .Where(t => t.Aktif)
                .OrderBy(t => t.Sira)
                .ToListAsync();
            return View(ekip);
        }

    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
