using System.Threading.Tasks;
using NvrOrganizer.DataAccess;
using NvrOrganizer.Model;
using System.Data.Entity;

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
    }
}
