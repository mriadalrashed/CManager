namespace CManager.Core.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }   = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValid()
        {
           return !string.IsNullOrWhiteSpace(FirstName) &&
                  !string.IsNullOrWhiteSpace(LastName) &&
                  IsValidEmail(Email) &&
                  !string.IsNullOrWhiteSpace(PhoneNumber) &&
                  !string.IsNullOrWhiteSpace(Address) &&
                  !string.IsNullOrWhiteSpace(City) &&
                  !string.IsNullOrWhiteSpace(Street) &&
                  !string.IsNullOrWhiteSpace(PostalCode);
        }

    }
} 

