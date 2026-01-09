// NOTE (AI Assistance Disclosure):
// The documentation comments in this file were created with assistance from an AI tool.
// CustomerValidator was introduced to apply the Single Responsibility Principle (SRP).
// It centralizes all customer-related validation logic in one place,
// avoiding duplication across services and controllers.
// This improves code maintainability, readability, and testability.
// Reference:
// Microsoft Docs – ArgumentException
// https://learn.microsoft.com/dotnet/api/system.argumentexception

using CManager.Core.Models;

namespace CManager.Application.Helpers
{
    // Provides validation methods for Customer objects
    public static class CustomerValidator
    {
        // Validates that all required customer fields are provided
        public static void ValidateRequiredFields(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer), "Customer object cannot be null.");

            // Validate required string fields

            if (string.IsNullOrWhiteSpace(customer.FirstName))
                throw new ArgumentException("FirstName is required.", nameof(customer.FirstName));
            
            if (string.IsNullOrWhiteSpace(customer.LastName))
                throw new ArgumentException("LastName is required.", nameof(customer.LastName));

            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Email is required.", nameof(customer.Email));

            if (string.IsNullOrWhiteSpace(customer.PhoneNumber))
                throw new ArgumentException("PhoneNumber is required.", nameof(customer.PhoneNumber));

            if (string.IsNullOrWhiteSpace(customer.Address))
                throw new ArgumentException("Address is required.", nameof(customer.Address));
            if (string.IsNullOrWhiteSpace(customer.City))
                throw new ArgumentException("City is required.", nameof(customer.City));

            if (string.IsNullOrWhiteSpace(customer.Street))
                 throw new ArgumentException("Street is required.", nameof(customer.Street));

            if (string.IsNullOrWhiteSpace(customer.PostalCode))
                 throw new ArgumentException("PostalCode is required.", nameof(customer.PostalCode));
        }

        // Validates the email format of a customer
        public static void ValidateEmail(Customer customer)
        {
            // Get customer email
            var Customeremail = customer.Email;

            // Check if the email format is valid
            if (!customer.IsValidEmail(Customeremail))
                throw new ArgumentException("Invalid email format.", nameof(customer.Email));
        } 
    }
}
