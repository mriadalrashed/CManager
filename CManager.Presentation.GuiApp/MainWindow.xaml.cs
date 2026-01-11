// AI DISCLOSURE & REFERENCES:
// Microsoft documentation was reviewed to understand WPF application startup,
// Window initialization, and basic dependency wiring without a DI container.
//
// AI was used as a discussion aid to clarify:
// - How to initialize services and ViewModels in WPF without using full DI frameworks
// - The implications of using reflection to inject private fields
// - Navigation patterns using ContentControl in MVVM applications
//
// The implementation, structure, and wiring logic were written and adjusted by me
//
// Purpose:
// This class initializes the application, configures file-based persistence,
// creates core services and ViewModels, and sets up navigation between views
// using a custom NavigationService.
//
// References:
// Microsoft Docs – WPF Application Startup
// https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/
//
// Microsoft Docs – ContentControl
// https://learn.microsoft.com/dotnet/api/system.windows.controls.contentcontrol
//
// Microsoft Docs – Reflection in .NET
// https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/reflection

using CManager.Application.Services;
using CManager.Core.Interfaces;
using CManager.Infrastructure.Data;
using CManager.Presentation.GuiApp.Services;
using CManager.Presentation.GuiApp.ViewModels;
using CManager.Presentation.GuiApp.Views;
using System;
using System.IO;
using System.Windows;

namespace CManager.Presentation.GuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>ش
    public partial class MainWindow : Window
    {
        private NavigationService _navigationService;
        public MainWindow()
        {
            InitializeComponent();
            IntializNavigation();
        }

        private void IntializNavigation()
        {
            // setup data file path
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDirectory = Path.Combine(appDataPath, "CManager");
            var filePath = Path.Combine(appDirectory, "data.json");
            Directory.CreateDirectory(appDirectory);

            // Create repository and service instances
            var repository = new CustomerRepository(filePath);
            var customerService = new CustomerService(repository);

            // first create the navigation service
            _navigationService = new NavigationService(null!,null!);

            var customerListViewModel = new CustomerListViewModel(customerService, _navigationService);
            var customerDetailViewModel = new CustomerDetailViewModel(customerService, _navigationService);
            //create view model with navigation service 
            typeof(NavigationService)
                .GetField("_customerListViewModel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                ?.SetValue(_navigationService, customerListViewModel);

            typeof(NavigationService)
                .GetField("_customerDetailViewModel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                ?.SetValue(_navigationService, customerDetailViewModel);

            // set content controle and navigate to customer list
            _navigationService.SetContentControl(MainContent);
        }

        // Navigates to the customer list view
        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateToCustomerList();
        }

        // Navigates to the create customer view
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
          _navigationService.NavigateToCreateCustomer();
        }
    }
}