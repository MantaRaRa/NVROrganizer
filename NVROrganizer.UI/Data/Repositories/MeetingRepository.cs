using System.Threading.Tasks;
using NvrOrganizer.DataAccess;
using NvrOrganizer.Model;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace NvrOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, NvrOrganizerDbContext>,
                                     IMeetingRepository
    {
        public MeetingRepository(NvrOrganizerDbContext context) : base(context)
        {
        }

        public async override Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
                 .Include(m => m.Nvrs)
                 .SingleAsync(m => m.Id == id);
        }

        public async Task<List<Nvr>> GetAllNvrsAsync()
        {
            return await Context.Set<Nvr>()
                .ToListAsync();
        }

        public async Task ReloadNvrAsync(int nvrId)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Nvr>()
                .SingleOrDefault(db => db.Entity.Id == nvrId);
            if (dbEntityEntry!=null)
            {
                await dbEntityEntry.ReloadAsync();
            }
        }
    }
}
