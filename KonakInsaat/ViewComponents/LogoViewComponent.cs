using KonakInsaat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KonakInsaat.ViewComponents
{
    public class LogoViewComponent : ViewComponent
    {
        private readonly KonakInsaatContext _context;

        public LogoViewComponent(KonakInsaatContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string style = null)
        {
            // Önce "Ana Logo" adlı markayı ara
            var anaLogo = await _context.Markalar
                .Where(m => m.MarkaAdi != null && m.MarkaAdi.ToLower().Contains("ana logo"))
                .FirstOrDefaultAsync();

            // Bulunamazsa ilk aktif markayı kullan
            if (anaLogo == null)
            {
                anaLogo = await _context.Markalar
                    .Where(m => m.Aktif)
                    .OrderBy(m => m.Sira)
                    .FirstOrDefaultAsync();
            }

            // Hala bulunamazsa varsayılan logo yolunu kullan
            string logoYolu = "/img/logo.png";
            if (anaLogo != null && !string.IsNullOrEmpty(anaLogo.ResimYolu))
            {
                logoYolu = anaLogo.ResimYolu;
            }

            ViewBag.Style = style;
            return View("Default", logoYolu);
        }
    }
}

