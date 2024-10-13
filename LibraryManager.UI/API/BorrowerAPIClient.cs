using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace LibraryManager.UI.API;

public class BorrowerAPIClient : IBorrowerAPIClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private const string PATH = "borrower";

    public BorrowerAPIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<List<Borrower>> GetAllBorrowersAsync()
    {
        var response = await _httpClient.GetAsync(PATH);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error getting all borrowers: {content}");

        return JsonSerializer.Deserialize<List<Borrower>>(content, _options);
    }

    public async Task<Borrower> GetBorrowerAsync(string email)
    {
        var response = await _httpClient.GetAsync($"{PATH}/{email}");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error getting borrower with email: {email}");

        return JsonSerializer.Deserialize<Borrower>(content, _options);
    }

    public async Task<Borrower> AddBorrowerAsync(AddBorrowerRequest borrower)
    {
        var response = await _httpClient.PostAsJsonAsync(PATH, borrower);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error adding borrower: {content}");

        return JsonSerializer.Deserialize<Borrower>(content, _options);
    }

    public async Task EditBorrowerAsync(EditBorrowerRequest borrower)
    {
        var response = await _httpClient.PutAsJsonAsync($"{PATH}/{borrower.BorrowerID}", borrower);
        
        if (!response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error editing borrower: {content}");
        }
    }

    public async Task DeleteBorrowerAsync(int borrowerID)
    {
        var response = await _httpClient.DeleteAsync($"{PATH}/{borrowerID}");

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error deleting borrower with ID {borrowerID}: {content}");
        }
    }
}
