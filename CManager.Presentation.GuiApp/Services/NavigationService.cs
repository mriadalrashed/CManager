using System;
using System.Windows.Controls;
using System.CManager.Presentation.GuiApp.Views;
using System.CManager.Presentation.GuiApp.ViewModels;
using CManager.Presentation.GuiApp.ViewModels;

namespace CManager.Presentation.GuiApp.Services
{
    public class NavigationService
    {
        private readonly CustomerListViewModel _customerListViewModel;
        private readonly CustomerDetailViewModel _customerDetailViewModel;
        private ContentControl _mainContentControl;

        public NavigationService(CustomerListViewModel customerListViewModel, CustomerDetailViewModel customerDetailViewModel)
        {
            _customerListViewModel = customerListViewModel;
            _customerDetailViewModel = customerDetailViewModel;
        }

        public void SetContentControl(ContentControl mainContentControl)
        {
            _mainContentControl = mainContentControl;
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

        public void NavigateToCustomerDetail(int customerId)
        {
            if (_mainContentControl == null)
                return;

            var view = new CustomerListView
            {
                DataContext = _customerDetailViewModel
            };
            _mainContentControl.Content = view;
            _customerDetailViewModel.LoadCustomerCommand.Execute(null);
        }

        public void NavigateToCreateCustomer()
        {
            if (_mainContentControl == null)
                return;
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
