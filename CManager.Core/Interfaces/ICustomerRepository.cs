using CManager.Core.Models;

namespace CManager.Core.Interfaces
{
    /// <summary>
    /// Defines data access operations for Customer entities.
    /// This interface represents the Repository layer responsibility,
    /// providing an abstraction for customer persistence and retrieval.
    /// </summary>
    /// <remarks>
    /// The repository is responsible only for data access logic such as
    /// storing, retrieving, updating, and deleting customer data.
    /// It does not contain business rules or validation logic.
    /// </remarks>
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
