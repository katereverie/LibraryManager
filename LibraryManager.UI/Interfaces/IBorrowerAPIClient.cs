using LibraryManager.UI.Models;

namespace LibraryManager.UI.Interfaces;

public interface IBorrowerAPIClient
{
    Task<List<Borrower>> GetAllBorrowersAsync();
    Task<Borrower> GetBorrowerAsync(string email);
    Task AddBorrowerAsync(AddBorrowerRequest borrower);
    Task EditBorrowerAsync(EditBorrowerRequest borrower);
    Task DeleteBorrowerAsync(int borrowerID);
}
