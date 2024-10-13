using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Models;
using System.Text.Json;

namespace LibraryManager.UI.API;

public class CheckoutAPIClient : ICheckoutAPIClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private const string PATH = "checkout";

    public CheckoutAPIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public Task CheckoutMediaAsync(int mediaId, string borrowerEmail)
    {
        throw new NotImplementedException();
    }

    public Task<List<Media>> GetAvailableMediaAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<CheckoutLog>> GetCheckoutLogAsync()
    {
        throw new NotImplementedException();
    }

    public Task ReturnMediaAsync(int checkoutLogId)
    {
        throw new NotImplementedException();
    }
}
