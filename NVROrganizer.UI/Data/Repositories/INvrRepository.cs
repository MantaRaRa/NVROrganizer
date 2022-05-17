using NvrOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NvrOrganizer.UI.Data.Repositories
{
    public interface INvrRepository:IGenericRepository<Nvr>
    {
      
        void RemovePhoneNumber(NvrPhoneNumber model);
        Task<bool> HasMeetingsAsync(int nvrId);
    }
}