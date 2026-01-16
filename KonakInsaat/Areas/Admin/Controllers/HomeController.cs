using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Geçici: Login sistemi hazır olana kadar
    public class HomeController : Controller
    {
        private readonly KonakInsaatContext _context;

        public HomeController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/Home/Index veya Admin/Home/AdminHome - Ana dashboard sayfası
        public async Task<IActionResult> Index()
        {
            return await AdminHome();
        }

        // GET: Admin/Home/AdminHome
        public async Task<IActionResult> AdminHome()
        {
            // Dashboard için istatistik bilgileri
            var mesajSayisi = await _context.Mesajlar.CountAsync(m => !m.Okundu);
            var hizmetSayisi = await _context.Hizmetler.CountAsync();
            var markaSayisi = await _context.Markalar.CountAsync();

            ViewBag.MesajSayisi = mesajSayisi;
            ViewBag.HizmetSayisi = hizmetSayisi;
            ViewBag.MarkaSayisi = markaSayisi;

            // Son 5 mesajı getir
            var sonMesajlar = await _context.Mesajlar
                .OrderByDescending(m => m.GonderimTarihi)
                .Take(5)
                .ToListAsync();

            return View("AdminHome", sonMesajlar);
        }

        // GET: Admin/Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Admin/Home/GetMessageCount - Ajax için mesaj sayısı
        [HttpGet]
        public async Task<IActionResult> GetMessageCount()
        {
            var mesajSayisi = await _context.Mesajlar.CountAsync(m => !m.Okundu);
            return Json(new { count = mesajSayisi });
        }

        // Eski route'lar için redirect'ler
        public IActionResult AdminSlider() => RedirectToAction("Index", "AdminSlider");
        public IActionResult AdminKurumsal() => RedirectToAction("Index", "AdminKurumsal");
        public IActionResult AdminProje() => RedirectToAction("Index", "AdminProje");
        public IActionResult AdminHizmet() => RedirectToAction("Index", "AdminHizmet");
    }
}
