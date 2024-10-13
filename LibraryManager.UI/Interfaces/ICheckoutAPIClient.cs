using LibraryManager.UI.Models;

namespace LibraryManager.UI.Interfaces;

public interface ICheckoutAPIClient
{
    Task<List<Media>> GetAvailableMediaAsync();
    Task<List<CheckoutLog>> GetCheckoutLogAsync();
    Task CheckoutMediaAsync(int mediaId, string borrowerEmail);
    Task ReturnMediaAsync(int checkoutLogId);
}
