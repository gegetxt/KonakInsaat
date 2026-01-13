using KonakInsaat.Areas.Admin.Models;
using KonakInsaat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KonakInsaat.ViewComponents
{
    public class HizmetlerViewComponent : ViewComponent
    {
        private readonly KonakInsaatContext _context;

        public HizmetlerViewComponent(KonakInsaatContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hizmetler = await _context.Hizmetler
                .Where(h => !string.IsNullOrEmpty(h.Baslik))
                .OrderBy(h => h.Id)
                .ToListAsync();

            return View("Default", hizmetler);
        }
    }
}

