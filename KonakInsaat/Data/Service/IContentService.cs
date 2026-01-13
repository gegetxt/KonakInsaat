using KonakInsaat.Models;

namespace KonakInsaat.Data.Service
{
    public interface IContentService
    {
        Task<IEnumerable<PageContent>> GetAllContentsAsync();
        Task<PageContent?> GetContentByIdAsync(int id);
        Task<PageContent?> GetContentByPageAndFieldAsync(string pageName, string fieldName, string language = "tr-TR");
        Task<PageContent> CreateContentAsync(PageContent content);
        Task<PageContent> UpdateContentAsync(PageContent content);
        Task<bool> DeleteContentAsync(int id);
    }
}

