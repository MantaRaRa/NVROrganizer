using NvrOrganizer.DataAccess;
using NvrOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvrOrganizer.UI.Data.Lookups
{
    public class LookupDataService : INvrLookupDataService, 
                                     IProgrammingLanguageLookupDataService,
                                     IMeetingLookupDataService
    {
        private Func<NvrOrganizerDbContext> _contextCreator;

        public LookupDataService(Func<NvrOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }
        public async Task<IEnumerable<LookupItem>> GetNvrLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Nvrs.AsNoTracking()
                    .Select(n =>
                    new LookupItem
                    {
                        Id = n.Id,
                        DisplayMember = n.FirstName + " " + n.LastName
                    })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking()
                    .Select(n =>
                    new LookupItem
                    {
                        Id = n.Id,
                        DisplayMember = n.Name
                    })
                    .ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                var items = await ctx.Meetings.AsNoTracking()
                  .Select(m =>
                     new LookupItem
                     {
                         Id = m.Id,
                         DisplayMember = m.Title
                     })
                  .ToListAsync();
                return items;
            }
        }

    }
}
