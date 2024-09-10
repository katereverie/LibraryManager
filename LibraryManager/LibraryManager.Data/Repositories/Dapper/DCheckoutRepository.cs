using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManager.Data.Repositories.Dapper
{
    public class DCheckoutRepository : ICheckoutRepository
    {
        private readonly string _connectionString;

        public DCheckoutRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(CheckoutLog newCheckoutLog)
        {
            throw new NotImplementedException();
        }

        public List<CheckoutLog> GetAllCheckedoutMedia()
        {
            throw new NotImplementedException();
        }

        public List<Media> GetAvailableMedia()
        {
            throw new NotImplementedException();
        }

        public Borrower? GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public List<CheckoutLog> GetCheckedoutMediaByBorrowerID(int borrowerID)
        {
            throw new NotImplementedException();
        }

        public List<CheckoutLog> GetCheckoutLogsByBorrowerID(int borrowerID)
        {
            throw new NotImplementedException();
        }

        public void Update(int checkoutLogID)
        {
            throw new NotImplementedException();
        }
    }
}
