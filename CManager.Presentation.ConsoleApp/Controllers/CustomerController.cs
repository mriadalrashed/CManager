using System;
using System.Collections.Generic;
using System.Text;
using CManager.Application.Services;
using CManager.Core.Models;

namespace CManager.Presentation.ConsoleApp.Controllers
{
    public class CustomerController
    {
        // Controller responsible for handling user interaction in the Console
        // and delegating operations to the CustomerService
        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        // Main menu loop that keeps the application running
        public void RunMenu()
        {
            while (true)
            {
                Console.Clear();
                DisplayMenu();
                // Read user choice
                var choice = GetMenuChoice();
                // Execute action based on user choice
                switch (choice)
                {
                    case 1:
                        CreateCustomer();
                        break;
                    case 2:
                        ViewAllCustomers();
                        break;
                    case 3:
                        ViewSpecificCustomer();
                        break;
                    case 4:
                        DeleteCustomer();
                        break;
                    case 5:
                        // Exit the application
                        Console.WriteLine("Thank you for using CManager. Goodbye!");
                        return;
                    default:
                        // Handle invalid input
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        // Displays the main menu options
        private void DisplayMenu()
        {
            Console.WriteLine("=== CManager - Customer Management System ===");
            Console.WriteLine("1. Create New Customer");
            Console.WriteLine("2. View All Customers");
            Console.WriteLine("3. View Specific Customer");
            Console.WriteLine("4. Delete Customer by Email");
            Console.WriteLine("5. Exit");
            Console.Write("\nEnter your choice (1-5): ");
        }

        // Reads and validates the menu choice input
        private int GetMenuChoice()
        {
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                return choice;
            }
            // Return invalid value if parsing fails
            return -1;
        }

        private void CreateCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Create New Customer ===");
            try
            {
                Console.Write("First name: ");
                var firstName = Console.ReadLine()?.Trim();
                Console.Write("Last name: ");
                var lastName = Console.ReadLine()?.Trim();
                Console.Write("Email: ");
                var email = Console.ReadLine()?.Trim();
                Console.Write("Phone Number: ");
                var phoneNumber = Console.ReadLine()?.Trim();
                Console.Write("Address: ");
                var address = Console.ReadLine()?.Trim();
                Console.Write("City: ");
                var city = Console.ReadLine()?.Trim();
                Console.Write("Street: ");
                var street = Console.ReadLine()?.Trim();
                Console.Write("Postal Code: ");
                var postalCode = Console.ReadLine()?.Trim();

                // Create a Customer object with the provided data
                var customerParameter = new Customer
                {
                    FirstName = firstName!,
                    LastName = lastName!,
                    Email = email!,
                    PhoneNumber = phoneNumber!,
                    Address = address!,
                    City = city!,
                    Street = street!,
                    PostalCode = postalCode!
                };

                // Call service layer to create the customer
                var customer = _customerService.CreateCustomer(customerParameter);

                // Display created customer details
                Console.WriteLine($"\nCustomer created successfully!");
                Console.WriteLine($"ID: {customer.Id}");
                Console.WriteLine($"First name: {customer.FirstName}");
                Console.WriteLine($"Last name: {customer.LastName}");
                Console.WriteLine($"Email: {customer.Email}");
                Console.WriteLine($"Phone Number: {customer.PhoneNumber}");
                Console.WriteLine($"Address: {customer.Address}");
                Console.WriteLine($"City: {customer.City}");
                Console.WriteLine($"Street: {customer.Street}");
                Console.WriteLine($"Postal Code: {customer.PostalCode}");
            }
            catch (Exception ex)
            {
                // Handle any errors during customer creation
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        // Displays all customers
        private void ViewAllCustomers()
        {
            Console.Clear();
            Console.WriteLine("=== All Customers ===");
            var customers = _customerService.GetAllCustomers();

            // Check if there are no customers
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }
            foreach (var customer in customers)
            {
                DisplayCustomer(customer);
                Console.WriteLine("---------------------------");
            }
            Console.WriteLine($"\nTotal Customers: {customers.Count}");
        }

        private void ViewSpecificCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== View Specific Customer ===");
            Console.Write("Enter customer email: ");
            var email = Console.ReadLine()?.Trim();
            // Validate email input
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email cannot be empty.");
                return;
            }
            var customer = _customerService.GetCustomerByEmail(email!);
            if (customer == null)
            {
                Console.WriteLine($"No Customer found with email: {email}");
            }
            else
            {
                Console.WriteLine("\nCustomer Found:");
                DisplayCustomer(customer, true);
            }
        }
        // Deletes a customer by email
        private void DeleteCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Delete Customer ===");
            Console.Write("Enter customer email to delete: ");
            var email = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("\nEmail cannot be empty.");
                return;
            }
            // Confirm deletion
            Console.Write($"Are you sure you want to delete customer with email '{email}'? (y/n): ");
            var confirmation = Console.ReadLine()?.Trim().ToLower();
            if (confirmation != "y")
            {
                Console.WriteLine("\nDeletion cancelled.");
                return;
            }

            try
            {
                // Attempt to delete customer
                var success = _customerService.DeleteCustomerByEmail(email!);
                if (success)
                    Console.WriteLine($"\nCustomer with email '{email}' has been deleted successfully.");
                else
                    Console.WriteLine($"No customer found with email: {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        
        private void DisplayCustomer(Customer customer, bool detailed = false)
        {
            Console.WriteLine($"First name: {customer.FirstName}");
            Console.WriteLine($"Last name: {customer.LastName}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine($"Phone Number: {customer.PhoneNumber}");
            Console.WriteLine($"Address: {customer.Address}");
            if (detailed)
            {
                Console.WriteLine($"City: {customer.City}");
                Console.WriteLine($"Street: {customer.Street}");
                Console.WriteLine($"Postal Code: {customer.PostalCode}");
            }
        }
    }
}