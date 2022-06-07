using System.Collections.Generic;
using System.Threading.Tasks;
using NvrOrganizer.Model;

namespace NvrOrganizer.UI.Data.Repositories
{
    public interface IMeetingRepository:IGenericRepository<Meeting>
    {
        Task<List<Nvr>> GetAllNvrsAsync();
        Task ReloadNvrAsync(int nvrId);
    }
}