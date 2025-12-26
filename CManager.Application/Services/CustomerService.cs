using System;
using System.Collections.Generic;
using System.Text;
using CManager.Core.Models;
using CManager.Core.Interfaces;
using CManager.Application.Helpers;

namespace CManager.Application.Services
{
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
            CusteomerValidator.validateRequiredFields(customer);
            CusteomerValidator.ValidateEmail(customer);
            customer.Id = GuidHelper.GenerateGuid();
            customer.Email = customer.Email.Trim().ToLower();
            customer.FirstName = customer.FirstName.Trim();
            customer.LastName = customer.LastName.Trim();
            customer.PhoneNumber = customer.PhoneNumber.Trim();
            customer.Address = customer.Address.Trim();
            customer.City = customer.City.Trim();
            customer.Street = customer.Street.Trim();
            customer.PostalCode = customer.PostalCode.Trim();

            if (_customerRepository.GetByEmail(customer.Email) != null)
                throw new InvalidOperationException("A customer with the same email already exists.");

            _customerRepository.Add(customer);
            _customerRepository.SaveChanges();

            return customer;
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll();
        }

        public Customer? GetCustomerById(Guid id)
        {
            return _customerRepository.GetById(id);
        }

        public Customer? GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            return _customerRepository.GetByEmail(email.Trim().ToLower());
        }

        public bool UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            CusteomerValidator.validateRequiredFields(customer);
            CusteomerValidator.ValidateEmail(customer);

            var existingCustomer = _customerRepository.GetById(customer.Id);
            if (existingCustomer == null)
                throw new InvalidOperationException("Customer not found.");

            var result = _customerRepository.Update(customer);

            if (result) _customerRepository.SaveChanges();
            return result;
        }

        public bool DeleteCustomer(Guid id)
        {
           if(id == Guid.Empty)
                throw new ArgumentException("Invalid customer ID.", nameof(id));
       
            var result = _customerRepository.Delete(id);
            if (result) _customerRepository.SaveChanges();
            return result;
        }


        public bool DeleteCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));
            var result = _customerRepository.DeleteByEmail(email.Trim().ToLower());
            if (result) _customerRepository.SaveChanges();
            return result;
        }

        public void SaveChanges()
        {
            _customerRepository.SaveChanges();
        }

    }
}
