using CManager.Core.Models;

namespace CManager.Core.Interfaces
{
    // Defines data access operations for Customer entities
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        List<Customer> GetAll();
        Customer? GetById(Guid id);
        Customer? GetByEmail(string email);
        bool Update(Customer customer);
        bool Delete(Guid id);
        bool DeleteByEmail(string email);
        void SaveChanges();
    }
}
