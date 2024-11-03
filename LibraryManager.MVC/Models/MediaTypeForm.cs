using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.MVC.Models;

public class MediaTypeForm
{
    [Display(Name = "Media Type")]
    public int? MediaTypeID { get; set; } // holds the selected media type's ID
    public string? Title { get; set; }
    public bool IsArchived { get; set; }
    public List<MediaModel>? Medias { get; set; }
    public SelectList MediaTypes { get; set; }
}
