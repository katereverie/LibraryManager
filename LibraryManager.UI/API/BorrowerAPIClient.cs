using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace LibraryManager.UI.API;

public class BorrowerAPIClient : IBorrowerAPIClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private const string PATH = "api/borrower";

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
            throw new HttpRequestException(content);

        return JsonSerializer.Deserialize<List<Borrower>>(content, _options);
    }

    public async Task<Borrower> GetBorrowerAsync(string email)
    {
        var response = await _httpClient.GetAsync($"{PATH}/{email}");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(content);

        return JsonSerializer.Deserialize<Borrower>(content, _options);
    }

    public async Task AddBorrowerAsync(AddBorrowerRequest borrower)
    {
        var response = await _httpClient.PostAsJsonAsync(PATH, borrower);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    if (doc.RootElement.TryGetProperty("errors", out var errorsElement))
                    {
                        var errorMessageList = new List<string>();

                        foreach (var error in errorsElement.EnumerateObject())
                        {
                            var field = error.Name;
                            var messages = error.Value.EnumerateArray().Select(e => e.GetString());

                            errorMessageList.Add($"{field}: {string.Join(", ", messages)}");
                        }

                        var errorMessage = string.Join("; ", errorMessageList);
                        throw new HttpRequestException($"Validation failed: {errorMessage}");
                    }
                }
            }
            catch (JsonException ex)
            {
                throw new HttpRequestException("An error occurred while processing the error response.", ex);
            }

            throw new HttpRequestException("An error occurred while processing your request.");
        }
    }

    public async Task EditBorrowerAsync(EditBorrowerRequest borrower)
    {
        var response = await _httpClient.PutAsJsonAsync($"{PATH}/{borrower.BorrowerID}", borrower);
        
        if (!response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content);
        }
    }

    public async Task DeleteBorrowerAsync(int borrowerID)
    {
        var response = await _httpClient.DeleteAsync($"{PATH}/{borrowerID}");

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content);
        }
    }

    public async Task<ViewBorrowerDTO> GetBorrowerWithLogsAsync(string email)
    {
        var response = await _httpClient.GetAsync($"{PATH}/{email}/logs");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(content);    
        }

        return JsonSerializer.Deserialize<ViewBorrowerDTO>(content, _options);
    }
}
