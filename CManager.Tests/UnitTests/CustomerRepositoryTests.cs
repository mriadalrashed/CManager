using System;
using System.IO;
using System.Linq;
using CManager.Core.Models;
using CManager.Infrastructure.Data;
using Xunit;

namespace CManager.Tests.UnitTests
{
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
                firstName = "John",
                lastName = "Doe",
                email = "john.doe@gmail.com",
                phoneNumber = "123-456-7890",
                address = "123 Main St, Anytown, USA",
                city = "Anytown",
                street = "123 Main St",
                postalCode = "33457"
            };

            // Act
            // add the customer and save changes to the file
            _repository.Add(customer);
            _repository.SaveChanges();

            // Assert
            // verify that the customer was added successfully
            var customers = _repository.GetAll().ToList();
            Assert.Single(customers);
            Assert.Equal("john.doe@gmail.com", customers[0].email);
        }

        [Fact]
        public void GetEmail_ShouldReturnCorrectCustomer()
        {
            // Arrange
            // create two customers
            var customer1 = new Customer
            {
                Id = Guid.NewGuid(),
                email = "customer1@gmail.com"
            };

            var customer2 = new Customer
            {
                Id = Guid.NewGuid(),
                email = "customer2@gmail.com"
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
                email = "delete@gmail.com"
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
                firstName = "Original Name",
                lastName = "Name",
                email = "original@gmail.com",
                phoneNumber = "0",
                address = "Original Address",
                city = "Original City",
                street = "0",
                postalCode = "00000"
            };

            _repository.Add(customer);
            _repository.SaveChanges();

            // create an updated customer with the same email
            var updatedCustomer = new Customer
            {
                Id = customer.Id,
                firstName = "Updated Name",
                lastName = "Updated Name",
                email = "original@gmail.com",
                phoneNumber = "0",
                address = "Updated Address",
                city = "Updated City",
                street = "0",
                postalCode = "00000"
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
            Assert.Equal("Updated Name", retrieved.firstName);
            Assert.Equal("Updated Name", retrieved.lastName);
            Assert.Equal("original@gmail.com", retrieved.email);
            Assert.Equal("Updated Address", retrieved.address);
            Assert.Equal("Updated City", retrieved.city);
            Assert.Equal("0", retrieved.phoneNumber);
            Assert.Equal("0", retrieved.street);
            Assert.Equal("00000", retrieved.postalCode);
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
                    email = $"test{i}@example.com"
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
