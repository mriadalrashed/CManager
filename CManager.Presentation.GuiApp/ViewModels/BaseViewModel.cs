using CommunityToolkit.Mvvm.ComponentModel;

namespace CManager.Presentation.GuiApp.ViewModels
{
    /// Base ViewModels wth common properties and methods
    public abstract partial class BaseViewModel : ObservableObject
    {
        // Title of the ViewModel
        [ObservableProperty]
        private string _title = string.Empty;

        // Status message to display to the user
        [ObservableProperty]
        private string _statutsMessage = string.Empty;

        // shows an error message
        protected void ShowError(string message)
        {
            StatutsMessage = $"Required: {message}";
        }

        // shows a success message
        protected void ShowSuccess(string message)
        {
            StatutsMessage = $"Success: {message}";
        }
    }
}