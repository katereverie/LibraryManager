﻿using LibraryManager.API.Models;
using LibraryManager.Core.DTOs;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers;

/// <summary>
/// Controller for handling media-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MediaController : Controller
{
    private readonly IMediaService _mediaService;
    private readonly ILogger<MediaController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MediaController"/> class, injecting <see cref="IMediaService"/> and <see cref="ILogger"/>.
    /// </summary>
    /// <param name="mediaService">Service for handling media-related operations.</param>
    /// <param name="logger">Logger for logging events and errors.</param>
    public MediaController(IMediaService mediaService, ILogger<MediaController> logger)
    {
        _mediaService = mediaService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of all existing media types.
    /// </summary>
    /// <returns>A list of <see cref="MediaType"/> objects.</returns>
    [HttpGet("types")]
    [ProducesResponseType(typeof(List<MediaType>), StatusCodes.Status200OK)]
    public IActionResult GetAllMediaTypes()
    {
        var result = _mediaService.GetAllMediaTypes();

        if (result.Ok)
        {
            _logger.LogInformation("All media types successfully retrieved.");
            return Ok(result.Data);
        }

        _logger.LogError("Error retrieving all media types. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Retrieves a list of media specified by its type ID.
    /// </summary>
    /// <param name="mediaTypeID">The ID number that uniquely identifies a media type.</param>
    /// <returns>A list of <see cref="Media"/> objects.</returns>
    [HttpGet("types/{mediaTypeID}")]
    [ProducesResponseType(typeof(List<Media>), StatusCodes.Status200OK)]
    public IActionResult GetMediaByType(int mediaTypeID)
    {
        var result = _mediaService.GetMediaByType(mediaTypeID);

        if (result.Ok)
        {
            _logger.LogInformation("All media by type ID {mediaTypeID} successfully retrieved.", mediaTypeID);
            return Ok(result.Data);
        }

        _logger.LogError("Error retrieving media by type ID {mediaTypeID}. Error: {ErrorMessage}", mediaTypeID, result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Retrieves a list of the top 3 media items defined by their checkout counts.
    /// </summary>
    /// <returns>A list of the top 3 <see cref="TopThreeMedia"/> objects.</returns>
    [HttpGet("top")]
    [ProducesResponseType(typeof(List<TopThreeMedia>), StatusCodes.Status200OK)]
    public IActionResult GetMostPopularMedia()
    {
        var result = _mediaService.GetTop3MostPopularMedia();

        if (result.Ok)
        {
            _logger.LogInformation("Top 3 media items retrieved successfully.");
            return Ok(result.Data);
        }

        _logger.LogError("Error retrieving top 3 media items. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Retrieves a list of media items that have been archived.
    /// </summary>
    /// <returns>A list of archived <see cref="Media"/> objects.</returns>
    [HttpGet("archived")]
    [ProducesResponseType(typeof(List<Media>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetArchivedMedia()
    {
        var result = _mediaService.GetAllArchivedMedia();

        if (result.Ok)
        {
            _logger.LogInformation("All archived media successfully retrieved.");
            return Ok(result.Data);
        }

        if (result.Message.Contains("Currently"))
        {
            _logger.LogWarning("No archived media found.");
            return NotFound(result.Message);
        }

        _logger.LogError("Error retrieving all archived media. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Adds a new media item specified by its media type ID and title.
    /// </summary>
    /// <param name="mediaToAdd">A JSON object for adding media, which includes a media's type ID and title.</param>
    /// <returns>A newly created <see cref="Media"/> object or an error response.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Media), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddMedia(AddMedia mediaToAdd)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when adding media. {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        var entity = new Media
        {
            MediaTypeID = mediaToAdd.MediaTypeID,
            Title = mediaToAdd.Title
        };

        var result = _mediaService.AddMedia(entity);

        if (result.Ok)
        {
            _logger.LogInformation("Media successfully added.");
            return Created();
        }

        _logger.LogError("Error adding media. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Archives a media item that has not been archived.
    /// </summary>
    /// <param name="mediaID">The ID number that uniquely identifies a media item.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpPut("{mediaID}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult ArchiveMedia(int mediaID)
    {
        var result = _mediaService.ArchiveMedia(mediaID);

        if (result.Ok)
        {
            _logger.LogInformation("Media archived successfully.");
            return NoContent();
        }

        _logger.LogError("Error archiving media. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }

    /// <summary>
    /// Edits the title and media type ID of a media item.
    /// </summary>
    /// <param name="mediaID">The ID number that uniquely identifies a media item.</param>
    /// <param name="editedMedia">A JSON object for editing media, which includes a media's ID, type ID, and title.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpPut("{mediaID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult EditMedia(int mediaID, EditMedia editedMedia)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when editing media. {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        var entity = new Media
        {
            MediaID = mediaID,
            Title = editedMedia.Title,
            MediaTypeID = editedMedia.MediaTypeID
        };

        var result = _mediaService.EditMedia(entity);

        if (result.Ok)
        {
            _logger.LogInformation("Media successfully edited.");
            return NoContent();
        }

        _logger.LogError("Error editing media. Error: {ErrorMessage}", result.Message);
        return StatusCode(500, "An unexpected error occurred while processing your request. Please try again later.");
    }
}
