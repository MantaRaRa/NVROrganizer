using System.Threading.Tasks;
using NvrOrganizer.Model;

namespace NvrOrganizer.UI.Data.Repositories
{
    public interface IProgrammingLanguageRepository
        : IGenericRepository<ProgrammingLanguage>
    {
        Task<bool> IsReferencedByNvrAsync(int programmingLanguageId);
    }
}
