using LibraryManager.Core.Entities;

namespace LibraryManager.MVC.Models;

public class MediaModel
{
    public int? MediaID { get; set; }
    public int? MediaTypeID { get; set; }
    public string? Title { get; set; }
    public bool IsArchived { get; set; }

    public MediaModel () { }

    public MediaModel (Media entity)
    {
        MediaID = entity.MediaID;
        MediaTypeID = entity.MediaTypeID;
        Title = entity.Title;
        IsArchived = entity.IsArchived;
    }

    public Media ToEntity()
    {
        return new Media
        {
            MediaID = MediaID ?? 0,
            MediaTypeID = MediaTypeID ?? 0,
            Title = Title ?? string.Empty,
            IsArchived = IsArchived
        };
    }
}