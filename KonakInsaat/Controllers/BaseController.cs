using KonakInsaat.Data.Service;
using KonakInsaat.Data;
using KonakInsaat.Models;
using KonakInsaat.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
namespace KonakInsaat.Controllers
{
    // Controllers/BaseController.cs
    public class BaseController : Controller
    {
        protected readonly IContactService _contactService;
        protected readonly KonakInsaatContext _context;
        
        public BaseController(IContactService contactService, KonakInsaatContext context)
        {
            _contactService = contactService;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> SubmitContactForm(ContactFormModel model)
        {
            // Farklı sayfalardaki formlarda alan adları değişebildiği için, eksikse formdan manuel eşleştir.
            if (!ModelState.IsValid)
            {
                var form = Request.HasFormContentType ? Request.Form : null;
                if (form != null)
                {
                    // İsim/Soyisim
                    model.NameSurname = FirstNonEmpty(
                        model.NameSurname,
                        form["adi"], form["name"], form["namesurname"], form["fullname"], form["FullName"]);

                    // E-posta
                    model.Email = FirstNonEmpty(
                        model.Email,
                        form["mail"], form["email"], form["Email"]);

                    // Telefon
                    model.Phone = FirstNonEmpty(
                        model.Phone,
                        form["tel"], form["phone"], form["Phone"]);

                    // Konu (gelmezse varsayılan)
                    model.Subject = FirstNonEmpty(
                        model.Subject,
                        form["subject"], form["konu"]) ?? "İletişim Formu";

                    // Mesaj (gelmezse kısa not)
                    model.Message = FirstNonEmpty(
                        model.Message,
                        form["message"], form["mesaj"]) ?? "Mesaj yok";
                }

                // Minimum doğrulama: İsim, E-posta veya Telefon’dan en az ikisi dolu olsun
                bool hasName = !string.IsNullOrWhiteSpace(model.NameSurname);
                bool hasEmail = !string.IsNullOrWhiteSpace(model.Email);
                bool hasPhone = !string.IsNullOrWhiteSpace(model.Phone);
                
                if (!(hasName && (hasEmail || hasPhone)))
                {
                    TempData["Error"] = "Lütfen adınızı ve iletişim bilgilerinizi giriniz.";
                    return RedirectToSafeReferer();
                }
            }

            // ContactFormModel'i Contacts tablosuna kaydet
            await _contactService.SaveContactRequest(model);
            
            // Aynı zamanda Mesaj tablosuna da kaydet (Admin paneli için)
            var mesaj = new Mesaj
            {
                AdSoyad = model.NameSurname,
                Email = model.Email,
                Telefon = model.Phone,
                Konu = model.Subject ?? "İletişim Formu",
                MesajIcerigi = model.Message ?? "Mesaj yok",
                GonderimTarihi = model.CreatedDate != default ? model.CreatedDate : DateTime.Now,
                Okundu = false,
                Yanitlandi = false,
                IpAdresi = HttpContext.Connection.RemoteIpAddress?.ToString()
            };
            
            await _context.Mesajlar.AddAsync(mesaj);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Mesajınız başarıyla gönderildi!";

            return RedirectToSafeReferer();
        }

        private IActionResult RedirectToSafeReferer()
        {
            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer))
            {
                return Redirect(referer);
            }
            return Redirect(Url.Action("Index", "Home")!);
        }

        private static string FirstNonEmpty(string fallback, params Microsoft.Extensions.Primitives.StringValues[] candidates)
        {
            if (!string.IsNullOrWhiteSpace(fallback)) return fallback;
            foreach (var candidate in candidates)
            {
                var value = candidate.ToString();
                if (!string.IsNullOrWhiteSpace(value)) return value;
            }
            return fallback;
        }
    }
}
