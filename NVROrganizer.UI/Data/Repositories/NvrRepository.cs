using NvrOrganizer.DataAccess;
using NvrOrganizer.Model;
using NvrOrganizer.UI.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NvrOrganizer.UI.Data.Repositories
{
    public class NvrRepository : GenericRepository<Nvr,NvrOrganizerDbContext>,
                                 INvrRepository
    {

        public NvrRepository(NvrOrganizerDbContext context)
            : base(context)
        {
           
        }

        public override async Task<Nvr> GetByIdAsync(int nvrId)
        {
            return await Context.Nvrs
                .Include(n => n.PhoneNumbers)
                .SingleAsync(n => n.Id == nvrId);
        }

        public async Task<bool> HasMeetingsAsync(int nvrId)
        {
            return await Context.Meetings.AsNoTracking()
                .Include(m => m.Nvrs)
                .AnyAsync(m => m.Nvrs.Any(n => n.Id == nvrId));
        }

        public void RemovePhoneNumber(NvrPhoneNumber model)
        {
            Context.NvrPhoneNumbers.Remove(model);
        }

    }
}
