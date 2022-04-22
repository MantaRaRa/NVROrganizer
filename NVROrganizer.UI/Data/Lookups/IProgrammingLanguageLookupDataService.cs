using System.Collections.Generic;
using System.Threading.Tasks;
using NvrOrganizer.Model;

namespace NvrOrganizer.UI.Data.Lookups
{
    public interface IProgrammingLanguageLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync();
    }
}