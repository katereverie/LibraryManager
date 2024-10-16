using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces;

public interface ICheckoutRepository
{
    List<Media> GetAvailableMedia();
    List<CheckoutLog> GetCheckoutLogsByBorrowerEmail(string email);
    List<CheckoutLog> GetCheckedoutMediaByBorrowerID(int borrowerID);
    List<CheckoutLog> GetAllCheckedoutMedia();
    void Update(int checkoutLogID);
    int Add(CheckoutLog newCheckoutLog);
}
