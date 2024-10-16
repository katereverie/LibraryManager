using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces;

public interface IBorrowerService
{
    Result<List<Borrower>> GetAllBorrowers();
    Result<Borrower> GetBorrower(string email);
    Result<List<CheckoutLog>> GetCheckoutLogsByEmail(string email);
    Result UpdateBorrower(Borrower borrower);
    Result AddBorrower(Borrower newBorrower);
    Result DeleteBorrower(Borrower Borrower);
}
