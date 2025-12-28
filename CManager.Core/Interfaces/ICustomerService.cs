using CManager.Core.Models;

namespace CManager.Core.Interfaces
{
    // Defines business operations related to customers
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
