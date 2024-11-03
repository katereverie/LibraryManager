using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManager.MVC.Controllers;

public class MediaController : Controller
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet]
    public IActionResult Search()
    {
        var result = _mediaService.GetAllMediaTypes();

        if (!result.Ok)
        {
            TempData["ErrorMessage"] = result.Message;
        }

        var model = new MediaTypeForm
        {
            MediaTypes = new SelectList(result.Data, "MediaTypeID", "MediaTypeName")
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Search(List<MediaTypeModel> mediaTypes, int mediaTypeID, string? title, bool includeArchived = false)
    {
        var result = _mediaService.GetMediaByType(mediaTypeID);
        List<Media> selectedMedia = new();

        if (includeArchived)
        {
            // pitfall: assuming result.Data is not null  
            selectedMedia = title == null 
                ? result.Data 
                : result.Data.FindAll(m => m.Title.Contains(title));
        }
        else
        {
            selectedMedia = title == null 
                ? result.Data.FindAll(m => m.IsArchived == false) 
                : result.Data.FindAll(m => m.IsArchived == false && m.Title.Contains(title));
        }

        var model = new MediaTypeForm
        {
            MediaTypes = new SelectList(mediaTypes, "MediaTypeID", "MediaTypeName"),
            Title = title,
            Medias = selectedMedia.Select(m => new MediaModel(m)).ToList(),
        };

        return View(model);
    }
}
