using KonakInsaat.Models;
using Microsoft.EntityFrameworkCore;

namespace KonakInsaat.Data.Service
{
    public class ContentService : IContentService
    {
        private readonly KonakInsaatContext _context;

        public ContentService(KonakInsaatContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PageContent>> GetAllContentsAsync()
        {
            return await _context.PageContents.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt).ToListAsync();
        }

        public async Task<PageContent?> GetContentByIdAsync(int id)
        {
            return await _context.PageContents.FindAsync(id);
        }

        public async Task<PageContent?> GetContentByPageAndFieldAsync(string pageName, string fieldName, string language = "tr-TR")
        {
            return await _context.PageContents
                .FirstOrDefaultAsync(x => x.PageName == pageName && x.FieldName == fieldName && x.Language == language);
        }

        public async Task<PageContent> CreateContentAsync(PageContent content)
        {
            content.CreatedAt = DateTime.Now;
            _context.PageContents.Add(content);
            await _context.SaveChangesAsync();
            return content;
        }

        public async Task<PageContent> UpdateContentAsync(PageContent content)
        {
            content.UpdatedAt = DateTime.Now;
            _context.PageContents.Update(content);
            await _context.SaveChangesAsync();
            return content;
        }

        public async Task<bool> DeleteContentAsync(int id)
        {
            var content = await _context.PageContents.FindAsync(id);
            if (content == null) return false;

            _context.PageContents.Remove(content);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

