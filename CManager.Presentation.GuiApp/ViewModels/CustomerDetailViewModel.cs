// AI DISCLOSURE & REFERENCES:
//
// The implementation of this ViewModel follows the MVVM pattern
// and uses CommunityToolkit.Mvvm for property change notification
// and command handling.
//
// AI was used only to assist in writing and refining documentation
// comments and to discuss architectural concepts such as MVVM,
// command binding, and navigation patterns. The implementation logic
// was written and understood by the author.
//
// Reasons & References:
//
// 1. MVVM Pattern:
//    Used to separate UI (View) from presentation logic (ViewModel),
//    improving testability and maintainability.
//    Reference:
//     https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm
//
// 2. ObservableObject (INotifyPropertyChanged):
//    Enables automatic UI updates when ViewModel properties change.
//    Reference:
//    https://learn.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanged
//
// 3. CommunityToolkit.Mvvm (ObservableProperty, RelayCommand):
//    Reduces boilerplate code for properties and commands in MVVM.
//    Reference:
//    https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/
//
// 4. Command-based interaction (RelayCommand):
//    Used instead of code-behind event handlers to follow MVVM best practices.
//    Reference:
//    https://learn.microsoft.com/dotnet/desktop/wpf/advanced/commanding-overview
//
// 5. Navigation using ContentControl:
//    Enables view switching without tight coupling between views.
//    Reference:
//    https://learn.microsoft.com/dotnet/api/system.windows.controls.contentcontrol


using System;
using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CManager.Application.Services;
using CManager.Presentation.GuiApp.Services;
using CManager.Core.Models;
using CommunityToolkit.Mvvm.Input;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media;

namespace CManager.Presentation.GuiApp.ViewModels
{

    /// <summary>
    /// ViewModel responsible for creating, editing, and displaying
    /// details of a single customer in the GUI application.
    /// </summary>
    /// <remarks>
    /// This ViewModel follows the MVVM pattern using CommunityToolkit.Mvvm.
    /// It coordinates between the UI (View), the business logic (CustomerService),
    /// and navigation logic (NavigationService).
    /// </remarks>
    public partial class CustomerDetailViewModel : BaseViewModel
    {
        private readonly CustomerService _customerService;
        private readonly NavigationService _navigationService;
        private Guid _customerId;

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

        /// <summary>
        /// Initializes a new instance of the CustomerDetailViewModel.
        /// </summary>
        /// <param name="customerService">
        /// Service used to perform business operations related to customers.
        /// </param>
        /// <param name="navigationService">
        /// Service responsible for navigating between views.
        /// </param>
        public CustomerDetailViewModel(CustomerService customerService, NavigationService navigationService)
        {
            _customerService = customerService;
            _navigationService = navigationService;
            Title = "Customer Details";
        }

        /// <summary>
        /// Loads customer data for either creating a new customer
        /// or editing an existing one.
        /// </summary>
        /// <param name="customerId">
        /// The unique identifier of the customer.
        /// If Guid.Empty, the ViewModel switches to create mode.
        /// </param>
        [RelayCommand]
        public void LoadCustomer(Guid customerId)
        {
            _customerId = customerId;
            if (customerId == Guid.Empty)
            {
                Title = "Creat New Customer";
                ClearForm();
            }
            else
            {
                Title = "Edit Customer";
                LoadExisttingCustomer();
            }
        }

        /// <summary>
        /// Loads an existing customer from the service
        /// and populates the ViewModel properties.
        /// </summary>
        public void LoadExisttingCustomer()
        {
            try
            {
                var customer = _customerService.GetCustomerById(_customerId);

                if (customer != null)
                {
                    FirstName = customer.FirstName;
                    LastName = customer.LastName;
                    Email = customer.Email;
                    PhoneNumber = customer.PhoneNumber;
                    Address = customer.Address;
                    City = customer.City;
                    Street = customer.Street;
                    PostalCode = customer.PostalCode;
                    ShowSuccess("Customer loaded successfully.");

                }
                else
                {
                    ShowError("Customer not found.");
                    ClearForm();
                }
            }

            catch (Exception ex)
            {
                ShowError($"Failed to load customer: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves the customer data.
        /// Creates a new customer if no ID exists,
        /// otherwise updates the existing customer.
        /// </summary>
        [RelayCommand]

        private void Save()
        {
            // Validate required fields
            if (!ValidateInput())
                return;
            try
            {
                if(_customerId == Guid.Empty)
                {
                    //create new customer
                    var newCustomer = new Customer
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        Address = Address,
                        City = City,
                        Street = Street,
                        PostalCode = PostalCode
                    };

                    //call service method that accepts a Customer object and saves it to the database.
                   var createdCustomer = _customerService.CreateCustomer(newCustomer);
                    //update the customer id with the newly created customer's id
                    _customerId = createdCustomer.Id;
                    ShowSuccess("Customer created successfully.");
                    ClearForm();
                    _navigationService.NavigateToCustomerList();
                }
                else                 {
                    //update existing customer
                    var customerToUpadte = new Customer
                    {
                        Id = _customerId,
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        Address = Address,
                        City = City,
                        Street = Street,
                        PostalCode = PostalCode
                    };
                    //call service for updating customer
                    var success = _customerService.UpdateCustomer(customerToUpadte);
                    if (success)
                    {
                        ShowSuccess("Customer updated successfully.");
                        _navigationService.NavigateToCustomerList();
                    }
                    else
                    {
                        ShowError("Failed to update customer.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Failed to save customer: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels the current operation,
        /// clears the form, and navigates back to the customer list.
        /// </summary>
        [RelayCommand]
        private void Cancel()
        {
            ClearForm();
            _navigationService.NavigateToCustomerList();
        }

        /// <summary>
        /// Clears all input fields in the form.
        /// Used when creating a new customer or cancelling an operation.
        /// </summary>
        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            PostalCode = string.Empty;
        }

        /// <summary>
        /// Validates that all required input fields are filled.
        /// Displays an error message if validation fails.
        /// </summary>
        /// <returns>
        /// True if all required fields are valid; otherwise false.
        /// </returns>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                ShowError("First Name is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                ShowError("Last Name is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                ShowError("Email is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                ShowError("Phone Number is required.");
                return false;
            }    
            if (string.IsNullOrWhiteSpace(Address)) 
            {
                ShowError("Address is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(City))
            {
                ShowError("City is required."); 
                return false;
            }
            if (string.IsNullOrWhiteSpace(Street)) 
            {
                ShowError("Street is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(PostalCode))
            {
                ShowError("Postal Code is required.");
                return false;
            }

            return true;
        }

    }

}

 



