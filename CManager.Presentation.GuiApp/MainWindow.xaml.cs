using System;
using System.IO;
using System.Windows;
using CManager.Application.Services;
using CManager.Infrastructure.Data;
using CManager.Presentation.GuiApp.ViewModels;
using CManager.Presentation.GuiApp.Services;
using CManager.Presentation.GuiApp.Views;

namespace CManager.Presentation.GuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateToCustomerList();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
          _navigationService.NavigateToCreateCustomer();
        }
    }
}