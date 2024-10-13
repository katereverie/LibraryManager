using LibraryManager.UI.Models;

namespace LibraryManager.UI.Interfaces;

public interface IMediaAPIClient
{
    Task<List<MediaType>> GetMediaTypesAsync();
    Task<List<Media>> GetMediaByTypeAsync(int mediaTypeID);
    Task<List<TopThreeMedia>> GetMostPopularMediaAsync();
    Task<Media> AddMediaAsync(AddMediaRequest media);
    Task EditMediaAsync(Media media);
    Task ArchiveMediaAsync(Media media);
    Task<List<Media>> GetArchivedMediaAsync();
}
