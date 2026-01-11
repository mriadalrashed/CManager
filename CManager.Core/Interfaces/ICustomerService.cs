using CManager.Core.Models;

namespace CManager.Core.Interfaces
{
    /// <summary>
    /// Defines business operations related to customer management.
    /// This interface represents the Service layer responsibility,
    /// handling business rules, validation, and coordination between
    /// the presentation layer and the data access layer.
    /// </summary>
    /// <remarks>
    /// The service layer does not handle data persistence details.
    /// It depends on repository abstractions and focuses only on
    /// business logic and workflow orchestration.
    /// </remarks>
    public interface ICustomerService
    {
        Customer CreateCustomer(Customer customer);

        List<Customer> GetAllCustomers();
        Customer? GetCustomerById(Guid id);
        Customer? GetCustomerByEmail(string email);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(Guid id);
        bool DeleteCustomerByEmail(string email);
        void SaveChanges();

    }
}
