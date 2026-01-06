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

        //initializes a new instance of the CustomerDetailViewModel class.
        public CustomerDetailViewModel(CustomerService customerService, NavigationService navigationService)
        {
            _customerService = customerService;
            _navigationService = navigationService;
            Title = "Customer Details";
        }

        // load customer Data for  editing
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

        //saves the customer data
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

        //cancel the operation 
        [RelayCommand]
        private void Cancel()
        {
            ClearForm();
            _navigationService.NavigateToCustomerList();
        }

        //clears the form fields
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

        //validates the input fields
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

 



