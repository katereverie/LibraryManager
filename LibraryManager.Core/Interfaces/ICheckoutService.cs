using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces;

public interface ICheckoutService
{
    Result<List<CheckoutLog>> GetAllCheckedoutMedia();
    Result<List<Media>> GetAvailableMedia();
    Result CheckoutMedia(int mediaID, string email);
    Result ReturnMedia(int checkoutLogID);
}
