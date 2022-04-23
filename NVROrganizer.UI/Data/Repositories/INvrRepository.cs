using NvrOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NvrOrganizer.UI.Data.Repositories
{
    public interface INvrRepository
    {
        Task<Nvr> GetByIdAsync(int nvrId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Nvr nvr);
        void Remove(Nvr model);
        void RemovePhoneNumber(NvrPhoneNumber model);
    }
}