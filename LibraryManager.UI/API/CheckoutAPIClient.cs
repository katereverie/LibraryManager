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

    public async Task<List<Media>> GetAvailableMediaAsync()
    {
        var response = await _httpClient.GetAsync(PATH);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error getting available medias: {content}");
        }

        return JsonSerializer.Deserialize<List<Media>>(content, _options);
    }

    public async Task<List<CheckoutLog>> GetCheckoutLogAsync()
    {
        var response = await _httpClient.GetAsync($"{PATH}/log");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error getting checkout logs: {content}");
        }

        return JsonSerializer.Deserialize<List<CheckoutLog>>(content, _options);
    }

    public async Task CheckoutMediaAsync(int mediaID, string borrowerEmail)
    {
        var response = await _httpClient.PostAsync($"{PATH}/media/{mediaID}/{borrowerEmail}", null);
        
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error checking out media: {content}");
        }
    }

    public async Task ReturnMediaAsync(int checkoutLogID)
    {
        var response = await _httpClient.PutAsync($"{PATH}/returns/{checkoutLogID}", null);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error returning media item: {content}");
        }
    }
}
