using System.Data.Entity;
using System.Threading.Tasks;
using NvrOrganizer.DataAccess;
using NvrOrganizer.Model;

namespace NvrOrganizer.UI.Data.Repositories
{
    public class ProgrammingLanguageRepository
        : GenericRepository<ProgrammingLanguage, NvrOrganizerDbContext>,
          IProgrammingLanguageRepository
    {
        public ProgrammingLanguageRepository(NvrOrganizerDbContext context)
            : base(context) 
        { 

        }

        public async Task<bool> IsReferencedByNvrAsync(int programmingLanguageId)
        {
            return await Context.Nvrs.AsNoTracking()
                .AnyAsync(f => f.FavoriteLanguageId == programmingLanguageId);
        }
    }
}
