// NOTE (AI Assistance Disclosure):
// The documentation comments in this file were written with assistance from an AI tool.
// Dependency Injection is used to decouple the service
// from the concrete repository implementation

using System;
using System.Collections.Generic;
using System.Text;
using CManager.Core.Models;
using CManager.Core.Interfaces;
using CManager.Application.Helpers;

namespace CManager.Application.Services
{
    /// <summary>
    /// Service responsible for handling customer-related business logic.
    /// </summary>
    /// <remarks>
    /// This service applies business rules, performs validation,
    /// and coordinates operations between the presentation layer
    /// and the repository layer, following SOLID principles.
    /// </remarks>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));
        }

        public Customer CreateCustomer(Customer customer)
        {
            // Validate required fields and email format
            CustomerValidator.ValidateRequiredFields(customer);
            CustomerValidator.ValidateEmail(customer);

            // Generate a unique identifier for the customer
            customer.Id = GuidHelper.GenerateGuid();

            // Normalize input data before persistence
            customer.Email = customer.Email.Trim().ToLower();
            customer.FirstName = customer.FirstName.Trim();
            customer.LastName = customer.LastName.Trim();
            customer.PhoneNumber = customer.PhoneNumber.Trim();
            customer.Address = customer.Address.Trim();
            customer.City = customer.City.Trim();
            customer.Street = customer.Street.Trim();
            customer.PostalCode = customer.PostalCode.Trim();

            // Business rule: prevent duplicate customers by email
            if (_customerRepository.GetByEmail(customer.Email) != null)
                throw new InvalidOperationException("A customer with the same email already exists.");

            _customerRepository.Add(customer);
            _customerRepository.SaveChanges();

            return customer;
        }

        /// <summary>
        /// Retrieves all customers from storage.
        /// </summary>
        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a customer by unique identifier.
        /// </summary>
        public Customer? GetCustomerById(Guid id)
        {
            return _customerRepository.GetById(id);
        }

        /// <summary>
        /// Retrieves a customer by email address.
        /// </summary>
        public Customer? GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            return _customerRepository.GetByEmail(email.Trim().ToLower());
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        public bool UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            CustomerValidator.ValidateRequiredFields(customer);
            CustomerValidator.ValidateEmail(customer);


            // Ensure the customer exists before updating
            var existingCustomer = _customerRepository.GetById(customer.Id);
            if (existingCustomer == null)
                throw new InvalidOperationException("Customer not found.");

            var result = _customerRepository.Update(customer);

            if (result) _customerRepository.SaveChanges();
            return result;
        }

        /// <summary>
        /// Deletes a customer by unique identifier.
        /// </summary>
        public bool DeleteCustomer(Guid id)
        {
           if(id == Guid.Empty)
                throw new ArgumentException("Invalid customer ID.", nameof(id));
       
            var result = _customerRepository.Delete(id);
            if (result) _customerRepository.SaveChanges();
            return result;
        }


        /// <summary>
        /// Deletes a customer by email address.
        /// </summary>
        public bool DeleteCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));
            var result = _customerRepository.DeleteByEmail(email.Trim().ToLower());
            if (result) _customerRepository.SaveChanges();
            return result;
        }

        /// <summary>
        /// Persists pending changes to storage.
        /// </summary>
        public void SaveChanges()
        {
            _customerRepository.SaveChanges();
        }

    }
}
