using KonakInsaat.Models;

namespace KonakInsaat.Data.Service
{
    public interface IContactService
    {
        Task<IEnumerable<ContactFormModel>> GetAll();
        Task SaveContactRequest(ContactFormModel model);
        Task<ContactFormModel?> GetById(int id);
        Task Update(ContactFormModel model);
        Task Delete(int id);
    }
}