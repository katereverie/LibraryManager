﻿using LibraryManager.Core;
using LibraryManager.Core.DTOs;
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

    public Result AddMedia(Media newMedia)
    {
        try
        {
            _mediaRepository.Add(newMedia);
            return ResultFactory.Success();

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

}
