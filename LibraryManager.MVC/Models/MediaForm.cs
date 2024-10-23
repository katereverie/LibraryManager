using LibraryManager.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.MVC.Models;

public class MediaForm
{
    public int? MediaID { get; set; }
    [Required]
    public int? MediaTypeID { get; set; }
    [Required]
    public string Title { get; set; }
    public bool IsArchived { get; set; }

    /// <summary>
    /// initialize default MediaForm DTO
    /// </summary>
    public MediaForm() { }

    /// <summary>
    /// transport data from Media entity to MediaForm DTO
    /// </summary>
    /// <param name="entity"></param>
    public MediaForm(Media entity)
    {
        MediaID = entity.MediaID;
        MediaTypeID = entity.MediaTypeID;
        Title = entity.Title;
        IsArchived = entity.IsArchived;
    }

    /// <summary>
    /// transport data from MediaForm DTO to Media entity
    /// </summary>
    /// <returns></returns>
    public Media ToEntity()
    {
        return new Media
        {
            MediaID = MediaID ?? 0,
            MediaTypeID = MediaTypeID ?? 0,
            Title = Title,
            IsArchived = IsArchived
        };
    }
}
