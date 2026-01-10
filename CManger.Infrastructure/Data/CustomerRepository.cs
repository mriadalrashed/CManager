// AI-assisted design discussion:
// AI was used to discuss applying the Repository Pattern
// and the Single Responsibility Principle (SRP).
// The repository is responsible only for data persistence,
// while JSON serialization is delegated to JsonFileHelper.

using System;
using CManager.Core.Interfaces;
using CManager.Core.Models;
using CManager.Infrastructure.Data;

namespace CManager.Infrastructure.Data
{

    /// <summary>
    /// Repository responsible for managing customer persistence.
    /// </summary>
    /// <remarks>
    /// This repository stores customer data in memory and persists
    /// changes to a JSON file using JsonFileHelper.
    /// It contains no business logic or validation rules.
    /// </remarks>
    public class CustomerRepository : ICustomerRepository
    {
        // Stores customers in memory
        private readonly List<Customer> _customers = new();

        // Path to the JSON data file
        private readonly string _filePath;
        
        // Initializes repository and loads customers from file
        public CustomerRepository(string filePath)
        {
            _filePath = filePath;
            _customers = LoadCustomers();
        }
        public void Add(Customer customer)
        {
           if (customer == null)
           {
               throw new ArgumentNullException(nameof(customer));
           }
           if (GetByEmail(customer.Email) !=null)
           {
               throw new ArgumentException($"Customer with the email {customer.Email} already exists.");
           }
              _customers.Add(customer);
        }

        // Returns all customers
        public List<Customer> GetAll()
        {
            return _customers;
        }

        public Customer? GetById(Guid id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }


        // AI-assisted clarification:
        // AI helped explain how to compare strings in a case-insensitive way in C#
        // using StringComparison.OrdinalIgnoreCase.
        // Reference (Microsoft Docs):
        // https://learn.microsoft.com/dotnet/api/system.stringcomparison
        public Customer? GetByEmail(string email)
        {
            return _customers.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        // Updates an existing customer if found
        // Returns false if the customer does not exist
        public bool Update(Customer customer)
        {
            var existingCustomer = GetById(customer.Id);
            if (existingCustomer == null)
            {
                return false;
            }
            _customers.Remove(existingCustomer);
            _customers.Add(customer);
            return true;
        }

        // Removes a customer by unique identifier
        public bool Delete(Guid id)
        {
            var customer = GetById(id);
            if (customer == null)
            {
                return false;
            }
            _customers.Remove(customer);
            return true;
        }

        public bool DeleteByEmail(string email)
        {
            var customer = GetByEmail(email);
            if (customer == null)
            {
                return false;
            }
            _customers.Remove(customer);
            return true;
        }

        // Persists the current in-memory state to the JSON file
        public void SaveChanges()
        {
            JsonFileHelper.WriteToJsonFile(_filePath, _customers);
        }

        // Loads data from the JSON file
        private List<Customer> LoadCustomers()
        {
            return JsonFileHelper.ReadFromJsonFile<Customer>(_filePath);
        }
    }
}
