// AI DISCLOSURE & REFERENCE:
// The decision to add repository-level tests was made after
// reviewing Microsoft documentation regarding testing data access layers
// and file-based persistence.
//
// AI was used as a discussion aid to better understand the concepts
// described in the documentation, not to generate the code.
//
// Purpose:
// These tests validate real JSON file persistence and data access behavior,
// which cannot be verified using mocked repositories.
//
// References:
// Microsoft Docs – Unit testing in .NET
// https://learn.microsoft.com/dotnet/core/testing/
// Microsoft Docs – IDisposable Interface
// https://learn.microsoft.com/dotnet/api/system.idisposable



using System;
using System.IO;
using System.Linq;
using CManager.Core.Models;
using CManager.Infrastructure.Data;
using Xunit;

namespace CManager.Tests.UnitTests
{
    // This test class verifies the behavior of the CustomerRepository,
    // focusing on real file-based JSON persistence and data access logic.
    // IDisposable is implemented to ensure proper cleanup of temporary
    // test files after each test run, maintaining test isolation and
    // preventing side effects between tests.

    public class CustomerRepositoryTests : IDisposable
    {
        // Path for a temporary JSON file used during testing
        private readonly string _testFilePath;

        // Instance of the repository being tested
        private readonly CustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            // Create a unique temporary file path for each test run
            _testFilePath = Path.Combine(
                Path.GetTempPath(),
                $"test_customer_{Guid.NewGuid()}.json"
            );

            // Initialize the repository with the test file path
            _repository = new CustomerRepository(_testFilePath);
        }

        public void Dispose()
        {
            // Clean up: delete the temporary file after each test
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Fact]
        public void AddShouldAddCustomerToList()
        {
            // Arrange
            // create a new customer object
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                PhoneNumber = "123-456-7890",
                Address = "123 Main St, Anytown, USA",
                City = "Anytown",
                Street = "123 Main St",
                PostalCode = "33457"
            };

            // Act
            // add the customer and save changes to the file
            _repository.Add(customer);
            _repository.SaveChanges();

            // Assert
            // verify that the customer was added successfully
            var customers = _repository.GetAll().ToList();
            Assert.Single(customers);
            Assert.Equal("john.doe@gmail.com", customers[0].Email);
        }

        [Fact]
        public void GetByEmail_ShouldReturnCorrectCustomer()
        {
            // Arrange
            // create two customers
            var customer1 = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "customer1@gmail.com"
            };

            var customer2 = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "customer2@gmail.com"
            };

            _repository.Add(customer1);
            _repository.Add(customer2);
            _repository.SaveChanges();

            // Act
            // retrieve customer by email
            var result = _repository.GetByEmail("customer2@gmail.com");

            // Assert
            // verify correct customer is returned
            Assert.NotNull(result);
            Assert.Equal(customer2.Id, result.Id);
        }

        [Fact]
        public void DeleteShouldRemoveCustomerFromList()
        {
            // Arrange
            // create and add a customer
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "delete@gmail.com"
            };

            _repository.Add(customer);
            _repository.SaveChanges();

            // Act
            // delete the customer by email
            var deleteResult = _repository.DeleteByEmail("delete@gmail.com");
            _repository.SaveChanges();

            // Assert
            // verify deletion was successful
            Assert.True(deleteResult);
            Assert.Null(_repository.GetByEmail("delete@gmail.com"));
        }

        [Fact]
        public void UpdateShouldModifyExistingCustomer()
        {
            // Arrange
            // create and add the original customer
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Original Name",
                LastName = "Name",
                Email = "original@gmail.com",
                PhoneNumber = "0",
                Address = "Original Address",
                City = "Original City",
                Street = "0",
                PostalCode = "00000"
            };

            _repository.Add(customer);
            _repository.SaveChanges();

            // create an updated customer with the same email
            var updatedCustomer = new Customer
            {
                Id = customer.Id,
                FirstName = "Updated Name",
                LastName = "Updated Name",
                Email = "original@gmail.com",
                PhoneNumber = "0",
                Address = "Updated Address",
                City = "Updated City",
                Street = "0",
                PostalCode = "00000"
            };

            // Act
            // update the customer
            var result = _repository.Update(updatedCustomer);
            _repository.SaveChanges();

            // Assert
            // verify update was successful
            Assert.True(result);

            var retrieved = _repository.GetByEmail("original@gmail.com");
            Assert.NotNull(retrieved);
            Assert.Equal("Updated Name", retrieved.FirstName);
            Assert.Equal("Updated Name", retrieved.LastName);
            Assert.Equal("original@gmail.com", retrieved.Email);
            Assert.Equal("Updated Address", retrieved.Address);
            Assert.Equal("Updated City", retrieved.City);
            Assert.Equal("0", retrieved.PhoneNumber);
            Assert.Equal("0", retrieved.Street);
            Assert.Equal("00000", retrieved.PostalCode);
        }

        [Fact]
        public void GetAllShouldReturnAllCustomers()
        {
            // Arrange
            // add multiple customers
            for (int i = 0; i < 3; i++)
            {
                _repository.Add(new Customer
                {
                    Id = Guid.NewGuid(),
                    Email = $"test{i}@example.com"
                });
            }

            _repository.SaveChanges();

            // Act
            // retrieve all customers
            var result = _repository.GetAll().ToList();

            // Assert
            // verify the correct number of customers is returned
            Assert.Equal(3, result.Count);
        }
    }
}
