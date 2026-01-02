using CManager.Application.Services;
using CManager.Core.Interfaces;
using CManager.Core.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace CManager.Tests.UnitTests
{
    // Unit tests for CustomerService
    public class CustomerServiceTests
    {
        // Mock instance of the customer repository
        private readonly Mock<ICustomerRepository> _mockRepository;

        // The service being tested
        private readonly CustomerService _customerService;

        // Test class constructor (runs before each test)
        public CustomerServiceTests()
        {
            // Initialize the mock repository
            _mockRepository = new Mock<ICustomerRepository>();

            // Inject the mocked repository into the service
            _customerService = new CustomerService(_mockRepository.Object);
        }

        [Fact]
        public void CreateCustomer_WithValidData_ShouldCreateCustomer()
        {
            //Arrange 
            var firstName = "Jane";
            var lastName = "Smith";
            var email = "JaneSmith@gmail.com";
            var phoneNumber = "1234567890";
            var address = "456 Elm St, Othertown, USA";
            var city = "Othertown";
            var street = "456 Elm St";
            var postalCode = "00000";

            // Mock repository behavior:
            // Return null to indicate that no customer exists with this email
            _mockRepository.Setup(r => r.GetByEmail(email))
                           .Returns((Customer?)null);

            // Expect Add and SaveChanges to be called during customer creation
            _mockRepository.Setup(r => r.Add(It.IsAny<Customer>())).Verifiable();
            _mockRepository.Setup(r => r.SaveChanges()).Verifiable();

            // Create a new customer object with valid data
            var newCustomer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                City = city,
                Street = street,
                PostalCode = postalCode
            };

            //Act
            // Call the method under test
            var result = _customerService.CreateCustomer(newCustomer);

            //Assert
            // Validate that the customer was created successfully
            Assert.NotNull(result);
            Assert.Equal(firstName, result.FirstName);
            Assert.Equal(lastName, result.LastName);

            // Email should be normalized to lowercase
            Assert.Equal(email.ToLower(), result.Email);

            Assert.Equal(phoneNumber, result.PhoneNumber);
            Assert.Equal(address, result.Address);
            Assert.Equal(city, result.City);
            Assert.Equal(street, result.Street);
            Assert.Equal(postalCode, result.PostalCode);

            // Ensure a new ID was generated
            Assert.NotEqual(Guid.Empty, result.Id);

            // Ensure the CreatedAt timestamp is valid
            Assert.True(result.CreatedAt <= DateTime.UtcNow);

            // Verify repository methods were called exactly once
            _mockRepository.Verify(r => r.Add(It.IsAny<Customer>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateCustomer_WithExistingEmail_ShouldThrowException()
        {
            //Arrange
            // Simulate an existing customer with the same email
            var existingEmail = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "ExistingUser@gmail.com"
            };

            // Repository returns an existing customer when searching by email
            _mockRepository.Setup(r => r.GetByEmail(existingEmail.Email))
                           .Returns(existingEmail);

            // New customer attempting to use an already existing email
            var newCustomer = new Customer
            {
                FirstName = "New",
                LastName = "User",
                Email = existingEmail.Email,
                PhoneNumber = "12345789",
                Address = "1 Oak St, Newtown, USA",
                City = "Newtown",
                Street = "789 Oak St",
                PostalCode = "00000"
            };

            //Act
            // Attempt to create the customer and expect an exception
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _customerService.CreateCustomer(newCustomer));

            //Assert
            // Validate the exception message
            Assert.Equal(
                "A customer with the provided email already exists.",
                exception.Message
            );
        }

        [Fact]
        public void CreateCustomer_WithInvalidEmail_ShouldThrowException()
        {
            //Arrange
            // Create a customer with an invalid email format
            var newCustomer = new Customer
            {
                FirstName = "Invalid",
                LastName = "EmailUser",
                Email = "invalid-email-format",
                PhoneNumber = "1234567890",
                Address = "789 Pine St, Sometown, USA",
                City = "Sometown",
                Street = "789 Pine St",
                PostalCode = "00000"
            };

            //Act
            // Attempt to create the customer and expect a FormatException
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _customerService.CreateCustomer(newCustomer));

            //Assert 
            // Validate the exception message
            Assert.Equal(
                "The provided email format is invalid.",
                exception.Message
            );
        }
    }
}
