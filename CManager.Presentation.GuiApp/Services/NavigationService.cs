// AI DISCLOSURE & REFERENCE:
// Microsoft documentation was reviewed to understand navigation patterns
// in WPF applications using ContentControl and MVVM.
//
// AI was used as a discussion aid to clarify how a centralized navigation
// service can coordinate view switching without breaking MVVM principles.
//
// Purpose:
// This service centralizes navigation logic for the GUI application,
// allowing ViewModels to request navigation without directly referencing Views.
// This improves separation of concerns and follows MVVM best practices.
//
// References:
// Microsoft Docs – ContentControl (WPF)
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.contentcontrol?view=windowsdesktop-10.0
//
//
// Microsoft Docs – MVVM pattern in WPF
// https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern


using System;
using System.Windows.Controls;
using CManager.Presentation.GuiApp.Views;
using CManager.Presentation.GuiApp.ViewModels;

namespace CManager.Presentation.GuiApp.Services
{
    /// <summary>
    /// Handles navigation between views in the WPF application.
    /// </summary>
    /// <remarks>
    /// This service uses a ContentControl to swap views at runtime,
    /// keeping navigation logic out of ViewModels and Views.
    /// This supports the MVVM pattern and the Single Responsibility Principle (SRP).
    /// </remarks>
    public class NavigationService
    {
        // Shared ViewModels used across different views
        private readonly CustomerListViewModel _customerListViewModel;
        private readonly CustomerDetailViewModel _customerDetailViewModel;

        // Holds the main ContentControl where views are displayed
        private ContentControl _mainContentControl;

        public NavigationService(CustomerListViewModel customerListViewModel, CustomerDetailViewModel customerDetailViewModel)
        {
            _customerListViewModel = customerListViewModel;
            _customerDetailViewModel = customerDetailViewModel;
        }

        /// Sets the main ContentControl used for navigation.
        /// This method should be called once during application startup.
        public void SetContentControl(ContentControl mainContentControl)
        {
            _mainContentControl = mainContentControl;
            // Show the customer list as the default view
            NavigateToCustomerList();
        }

        public void NavigateToCustomerList()
        {
            var customerListView = new CustomerListView
            {
                DataContext = _customerListViewModel
            };
            _mainContentControl.Content = customerListView;
        }

        /// Navigates to the customer list view.
        /// This method creates the view, assigns its ViewModel,
        /// and displays it inside the ContentControl.
        public void NavigateToCustomerDetail(int customerId)
        {
            if (_mainContentControl == null)
                return;

            var view = new CustomerListView
            {
                DataContext = _customerDetailViewModel
            };
            _mainContentControl.Content = view;
            // Execute command to load customer data
            _customerDetailViewModel.LoadCustomerCommand.Execute(null);
        }

        public void NavigateToCreateCustomer()
        {
            if (_mainContentControl == null)
                return;
            // Prepare ViewModel for new customer creation
            _customerDetailViewModel.LoadCustomer(Guid.Empty);
            var view = new CustomerDetailView
            {
                DataContext = _customerDetailViewModel
            };
            _mainContentControl.Content = view;
        }

        public void NavigateToEditCustomer(Guid customerId)
        {
            if (_mainContentControl == null)
                return;
            _customerDetailViewModel.LoadCustomer(customerId);
            var view = new CustomerDetailView
            {
                DataContext = _customerDetailViewModel
            };
            _mainContentControl.Content = view;
        }
    }
}
