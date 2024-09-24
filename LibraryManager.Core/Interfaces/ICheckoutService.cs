using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces
{
    public interface ICheckoutService
    {
        Result<List<CheckoutLog>> GetAllCheckedoutMedia();
        Result<List<Media>> GetAvailableMedia();
        Result<List<CheckoutLog>> GetCheckoutLogsByBorrowerID(int borrowerID);
        Result<List<CheckoutLog>> GetCheckedOutMediaByBorrowerID(int borrowerID);
        Result<int> CheckoutMedia(CheckoutLog newCheckoutLog);
        Result<Borrower> GetBorrowerByEmail(string email);
        Result<Media> GetMediaByID(int mediaID);
        Result ReturnMedia(int checkoutLogID);
    }
}
