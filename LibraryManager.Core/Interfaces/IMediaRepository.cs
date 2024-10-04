using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces;

public interface IMediaRepository
{
    int Add(Media newMedia);
    void Update(Media request);
    void Archive(int mediaID);
    List<Media> GetAll();
    List<Media> GetAllUnarchived();
    List<Media> GetUnarchivedByType(int typeID);
    List<Media> GetAllArchived();
    List<Media> GetByType(int mediaTypeID);
    List<MediaType> GetAllMediaTypes();
    List<TopThreeMedia> GetTopThreeMostPopularMedia();
    Media? GetByID(int mediaId);
}
