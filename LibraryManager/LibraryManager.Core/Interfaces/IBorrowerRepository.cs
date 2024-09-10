using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces
{
    public interface IBorrowerRepository
    {
        int Add(Borrower newBorrower);
        void Delete(Borrower borrower);
        void Update(Borrower borrower);
        List<Borrower> GetAll();
        List<CheckoutLog> GetCheckoutLogs(Borrower borrower);

        Borrower? GetByEmail(string email);
    }
}
