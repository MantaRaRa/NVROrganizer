using NvrOrganizer.Model;
using System.Collections.Generic;

namespace NvrOrganizer.UI.Data.Repositories
{
    public interface INvrRepository:IGenericRepository<Nvr>
    {
      
        void RemovePhoneNumber(NvrPhoneNumber model);
    }
}