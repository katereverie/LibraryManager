using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Models;
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

    public Task<Borrower> AddBorrowerAsync(AddBorrowerRequest borrower)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBorrowerAsync(int borrowerID)
    {
        throw new NotImplementedException();
    }

    public Task EditBorrowerAsync(EditBorrowerRequest borrower)
    {
        throw new NotImplementedException();
    }

    public Task<List<Borrower>> GetAllBorrowersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Borrower> GetBorrowerAsync(string email)
    {
        throw new NotImplementedException();
    }
}
