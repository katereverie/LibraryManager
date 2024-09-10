using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManager.Data.Repositories.Dapper
{
    public class DBorrowerRepository : IBorrowerRepository
    {
        private readonly string _connectionString;
        public DBorrowerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(Borrower newBorrower)
        {
            throw new NotImplementedException();
        }

        public void Delete(Borrower borrower)
        {
            throw new NotImplementedException();
        }

        public List<Borrower> GetAll()
        {
            throw new NotImplementedException();
        }

        public Borrower? GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public List<CheckoutLog> GetCheckoutLogs(Borrower borrower)
        {
            throw new NotImplementedException();
        }

        public void Update(Borrower borrower)
        {
            throw new NotImplementedException();
        }
    }
}
