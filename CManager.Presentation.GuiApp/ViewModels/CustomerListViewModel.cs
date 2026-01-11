// AI DISCLOSURE & REFERENCES:
// Microsoft documentation was reviewed to understand MVVM patterns,
// ObservableCollection usage, and command handling with CommunityToolkit.Mvvm.
//
// AI was used as a discussion aid to help structure documentation comments
// and clarify best practices for ViewModel responsibilities, commands,
// and UI state handling (IsBusy, SelectedItem patterns).
//
// Purpose:
// This ViewModel manages customer list presentation logic,
// including loading, deleting, and navigating to customer details,
// following MVVM and separation of concerns.



using CManager.Application.Services;
using CManager.Core.Models;
using CManager.Presentation.GuiApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;


namespace CManager.Presentation.GuiApp.ViewModels
{
    /// <summary>
    /// ViewModel responsible for displaying and managing a list of customers.
    /// Handles loading customers, deleting a selected customer,
    /// and navigating to customer details.
    /// </summary>
    public partial class CustomerListViewModel : BaseViewModel
    {
        private readonly CustomerService _customerService;
        private readonly NavigationService _navigationService;

        [ObservableProperty]
        private CustomerViewModel? _selectedCustomer;

        //get the collection of customers
        public ObservableCollection<CustomerViewModel> Customers { get; } = new();

        //get and set a value indicate viewmodel is busy or not
        [ObservableProperty]
        private bool _isBusy;

        //initialize a new instance of CustomerListViewModel class
        public CustomerListViewModel(CustomerService customerService, NavigationService navigationService)
        {
            _customerService = customerService;
            _navigationService = navigationService;
            Title = "Customers List";

            //load customers from the service
            LoadCustomer();
        }

        // AI note:
        // AI was used to clarify how ObservableCollection automatically updates the UI
        // when items are added or removed.

        //load all customers from the service
        [RelayCommand]
        private void LoadCustomer()
        {
            IsBusy = true;
            StatutsMessage = "Loading customers...";
            try
            {
                Customers.Clear();
                var customers = _customerService.GetAllCustomers();

                foreach (var customer in customers)
                {
                    Customers.Add(new CustomerViewModel(customer));
                }

                ShowSuccess($"Loaded {Customers.Count} customers.");
            }
            catch (Exception ex)
            {
                ShowError($"Failed to load customers:{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        //delete the selected customer
        [RelayCommand(CanExecute = nameof(CanDeleteCustomer))]

        private void DeleteCustomer()
        {
            if (SelectedCustomer == null)
                return;
            IsBusy = true;
            StatutsMessage = "Deleting customer...";
            try
            {
                var success = _customerService.DeleteCustomerByEmail(SelectedCustomer.Email);
                if (success)
                {
                    Customers.Remove(SelectedCustomer);
                    SelectedCustomer = null;
                    ShowSuccess("Customer deleted successfully.");
                }
                else
                {
                    ShowError("Failed to delete customer.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Failded to delete customer:{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        //determine if a customer can be deleted or not
        private bool CanDeleteCustomer()
        {
            return SelectedCustomer != null && !IsBusy;
        }

        //View details of the selected customer
        [RelayCommand(CanExecute = nameof(CanViewDetails))]
        private void ViewDetails()
        {
            if (SelectedCustomer != null)
            {
                _navigationService.NavigateToEditCustomer(SelectedCustomer.Id);
            }
        }

        //determine if details of a customer can be viewed or not
        private bool CanViewDetails()
        {
            return SelectedCustomer != null;
        }

        //call when SelectedCustomer property changes
        partial void OnSelectedCustomerChanged(CustomerViewModel? value)
        {
            DeleteCustomerCommand.NotifyCanExecuteChanged();
            ViewDetailsCommand.NotifyCanExecuteChanged();
        }
    }

    //view model for customer
    public partial class CustomerViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;
        [ObservableProperty]
        private string _firstName = string.Empty;
        [ObservableProperty]
        private string _lastName = string.Empty;
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private string _phoneNumber = string.Empty;
        [ObservableProperty]
        private string _address = string.Empty;
        [ObservableProperty]
        private string _city = string.Empty;
        [ObservableProperty]
        private string _street = string.Empty;
        [ObservableProperty]
        private string _postalCode = string.Empty;
        [ObservableProperty]
        private DateTime _createdAt;

        //initialize a new instance of CustomerViewModel class
        public CustomerViewModel(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            Id = customer.Id;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Email = customer.Email;
            PhoneNumber = customer.PhoneNumber;
            Address = customer.Address;
            City = customer.City;
            Street = customer.Street;
            PostalCode = customer.PostalCode;
            CreatedAt = customer.CreatedAt;
        }

        //Defaine a constructor for xml design time data

        public CustomerViewModel()
        {
            //Default constructor for xaml binding
        }
    }

}
