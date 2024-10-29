using LibraryManager.Core.DTOs;
using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces;

public interface IMediaService
{
    Result<List<Media>> GetMediaByType(int typeID);
    Result<List<Media>> GetAllArchivedMedia();
    Result<List<MediaType>> GetAllMediaTypes();
    Result<List<TopThreeMedia>> GetTop3MostPopularMedia();
    Result AddMedia(Media newMedia);
    Result ArchiveMedia(int mediaID);
    Result EditMedia(Media request);
}
