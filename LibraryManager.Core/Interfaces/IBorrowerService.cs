using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces;

public interface IBorrowerService
{
    Result<List<Borrower>> GetAllBorrowers();
    Result<Borrower> GetBorrower(string email);
    Result<ViewBorrowerDTO> GetBorrowerWithLogs(string email);
    Result UpdateBorrower(Borrower borrower);
    Result<int> AddBorrower(Borrower newBorrower);
    Result DeleteBorrower(Borrower Borrower);
}
