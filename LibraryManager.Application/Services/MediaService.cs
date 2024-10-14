using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;

namespace LibraryManager.Application.Services;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;

    public MediaService(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public Result<Media> AddMedia(Media newMedia)
    {
        try
        {
            _mediaRepository.Add(newMedia);
            return ResultFactory.Success(newMedia);

        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Media>(ex.Message);
        }
    }

    public Result ArchiveMedia(int mediaID)
    {
        try
        {
            _mediaRepository.Archive(mediaID);

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result EditMedia(Media request)
    {
        try
        {
            _mediaRepository.Update(request);

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail(ex.Message);
        }
    }

    public Result<List<Media>> GetAllArchivedMedia()
    {
        try
        {
            var list = _mediaRepository.GetAllArchived();

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<Media>>("Currently, there is no archived media.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Media>>(ex.Message);
        }
    }

    public Result<List<MediaType>> GetAllMediaTypes()
    {
        try
        {
            var list = _mediaRepository.GetAllMediaTypes();

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<MediaType>>("Currently, there is not registered media type.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<MediaType>>(ex.Message);
        }
    }

    public Result<Media> GetMediaByID(int mediaID)
    {
        try
        {
            var media = _mediaRepository.GetByID(mediaID);

            return media != null
                ? ResultFactory.Success(media)
                : ResultFactory.Fail<Media>($"Media with ID {mediaID} not found");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<Media>(ex.Message);
        }
    }

    public Result<List<Media>> GetMediaByType(int typeId)
    {
        try
        {
            var list = _mediaRepository.GetByType(typeId);

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<Media>>("Media of this type not found.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Media>>(ex.Message);
        }
    }

    public Result<List<TopThreeMedia>> GetTop3MostPopularMedia()
    {
        try
        {
            var list = _mediaRepository.GetTopThreeMostPopularMedia();

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<TopThreeMedia>>("Top 3 Most Popular Media not found.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<TopThreeMedia>>(ex.Message);
        }
    }

    public Result<List<Media>> GetUnarchivedMediaByType(int typeID)
    {
        try
        {
            var list = _mediaRepository.GetUnarchivedByType(typeID);

            return list.Any()
                ? ResultFactory.Success(list)
                : ResultFactory.Fail<List<Media>>("No unarchived media of this type is found.");
        }
        catch (Exception ex)
        {
            return ResultFactory.Fail<List<Media>>(ex.Message);
        }
    }
}
