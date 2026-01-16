using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KonakInsaat.Models;
using Microsoft.EntityFrameworkCore;

namespace KonakInsaat.Data.Service
{
    public class ContactService : IContactService
    {
        private readonly KonakInsaatContext _context;

        public ContactService(KonakInsaatContext context)
        {
            _context = context;
        }

        // 📩 Tüm mesajları getirir (tarihe göre azalan sıralı)
        public async Task<IEnumerable<ContactFormModel>> GetAll()
        {
            return await _context.Contacts
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        // 🆕 Yeni mesaj kaydeder
        public async Task SaveContactRequest(ContactFormModel model)
        {
            // Tarih alanı boşsa sistem zamanı ekle
            if (model.CreatedDate == default)
                model.CreatedDate = DateTime.Now;

            await _context.Contacts.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        // 🔍 ID'ye göre mesaj getirir
        public async Task<ContactFormModel?> GetById(int id)
        {
            return await _context.Contacts
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // ✏️ Mesaj günceller (örneğin silme, önemli olarak işaretleme)
        public async Task Update(ContactFormModel model)
        {
            _context.Contacts.Update(model);
            await _context.SaveChangesAsync();
        }

        // 🗑️ Mesajı tamamen siler (opsiyonel - normalde soft delete önerilir)
        public async Task Delete(int id)
        {
            var entity = await _context.Contacts.FindAsync(id);
            if (entity != null)
            {
                _context.Contacts.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}