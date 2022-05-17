namespace NvrOrganizer.UI.View.Services
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowOKCancelDialog(string text, string title);
        void ShowInfoDialog(string text);
    }
}