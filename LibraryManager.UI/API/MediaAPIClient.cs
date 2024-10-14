using LibraryManager.UI.Interfaces;
using LibraryManager.UI.Models;
using System.Net.Http.Json;
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

    public async Task<List<Media>> GetMediaByTypeAsync(int mediaTypeID)
    {
        var response = await _httpClient.GetAsync($"{PATH}/types/{mediaTypeID}");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error getting media by type ID {mediaTypeID}: {content}");
        }

        return JsonSerializer.Deserialize<List<Media>>(content, _options);
    }

    public async Task<Media> AddMediaAsync(AddMediaRequest media)
    {
        var response = await _httpClient.PostAsJsonAsync(PATH, media);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error adding media: {content}");
        }

        return JsonSerializer.Deserialize<Media>(content, _options);
    }

    public async Task ArchiveMediaAsync(Media media)
    {
        var response = await _httpClient.PutAsJsonAsync($"{PATH}/{media.MediaID}/archive", media);
        
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error archiving media: {content}");
        }
    }

    public async Task EditMediaAsync(Media media)
    {
        var response = await _httpClient.PutAsJsonAsync($"{PATH}/{media.MediaID}", media);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error editing media: {content}");
        }
    }

    public async Task<List<Media>> GetArchivedMediaAsync()
    {
        var response = await _httpClient.GetAsync($"{PATH}/archived");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error getting archived media: {content}");
        }

        return JsonSerializer.Deserialize<List<Media>>(content, _options);
    }

    public async Task<List<MediaType>> GetMediaTypesAsync()
    {
        var response = await _httpClient.GetAsync($"{PATH}/types");
        var content = await response.Content.ReadAsStringAsync(); 

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error getting media types: {content}");
        }

        return JsonSerializer.Deserialize<List<MediaType>>(content, _options);
    }

    public async Task<List<TopThreeMedia>> GetMostPopularMediaAsync()
    {
        var response = await _httpClient.GetAsync($"{PATH}/top");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error getting top 3 media items: {content}");
        }

        return JsonSerializer.Deserialize<List<TopThreeMedia>>(content, _options);
    }
}
