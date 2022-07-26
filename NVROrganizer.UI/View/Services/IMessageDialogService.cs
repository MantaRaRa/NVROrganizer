using System.Threading.Tasks;

namespace NvrOrganizer.UI.View.Services
{
    public interface IMessageDialogService
    {
        Task<MessageDialogResult> ShowOKCancelDialogAsync(string text, string title);
        Task ShowInfoDialogAsync(string text);
    }
}