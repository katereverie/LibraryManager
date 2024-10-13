using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Models;
using System.Text.Json;

namespace LibraryManager.UI.API;

public class MediaAPIClient : IMediaAPIClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private const string PATH = "media";

    public MediaAPIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public Task<Media> AddMediaAsync(AddMediaRequest media)
    {
        throw new NotImplementedException();
    }

    public Task ArchiveMediaAsync(Media media)
    {
        throw new NotImplementedException();
    }

    public Task EditMediaAsync(Media media)
    {
        throw new NotImplementedException();
    }

    public Task<List<Media>> GetArchivedMediaAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Media>> GetMediaByTypeAsync(int mediaTypeID)
    {
        throw new NotImplementedException();
    }

    public Task<List<MediaType>> GetMediaTypesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<TopThreeMedia>> GetMostPopularMediaAsync()
    {
        throw new NotImplementedException();
    }
}
