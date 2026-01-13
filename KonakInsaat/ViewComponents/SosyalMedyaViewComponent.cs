using KonakInsaat.Areas.Admin.Models;
using KonakInsaat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KonakInsaat.ViewComponents
{
    public class SosyalMedyaViewComponent : ViewComponent
    {
        private readonly KonakInsaatContext _context;

        public SosyalMedyaViewComponent(KonakInsaatContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sosyalMedyaHesaplari = await _context.SosyalMedya
                .Where(s => s.Aktif && !string.IsNullOrEmpty(s.Link))
                .OrderBy(s => s.Sira)
                .ToListAsync();

            return View("Default", sosyalMedyaHesaplari);
        }
    }
}








