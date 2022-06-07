using System.Threading.Tasks;

namespace NvrOrganizer.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int id);
        bool HasChanges { get; }
        int Id { get; }
    }
}