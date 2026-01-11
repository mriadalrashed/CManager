// AI DISCLOSURE & REFERENCE:
// Microsoft documentation was reviewed to understand how ObservableObject
// and source-generated properties work in CommunityToolkit.Mvvm.
//
// AI was used as a discussion aid to clarify how a shared BaseViewModel
// can reduce duplicated code across multiple ViewModels.
// The implementation and structure were written by the author.
//
// Purpose:
// This base class provides common properties and helper methods
// that are shared across all ViewModels in the GUI application,
// following the DRY principle and MVVM best practices.
//
// References:
// Microsoft Docs – MVVM pattern
// https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm
//
// CommunityToolkit.Mvvm – ObservableObject
// https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/observableobject

using CommunityToolkit.Mvvm.ComponentModel;

namespace CManager.Presentation.GuiApp.ViewModels
{
    /// <summary>
    /// Base ViewModel containing common properties and functionality
    /// shared by all ViewModels in the application.
    /// </summary>
    /// <remarks>
    /// This class centralizes UI-related state such as titles and status messages,
    /// reducing duplication and enforcing consistent behavior across Views.
    /// </remarks>
    public abstract partial class BaseViewModel : ObservableObject
    {
        /// Title of the current view.
        /// Automatically raises PropertyChanged when updated.
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

        /// Displays a formatted success message in the UI.
        /// Intended to be used by derived ViewModels.
        protected void ShowSuccess(string message)
        {
            StatutsMessage = $"Success: {message}";
        }
    }
}