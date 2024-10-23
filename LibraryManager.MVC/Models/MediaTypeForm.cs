using System.ComponentModel.DataAnnotations;

namespace LibraryManager.MVC.Models;

public class MediaTypeForm
{
    public int MediaTypeID { get; set; }
    [Required]
    public string MediaTypeName { get; set; }
}
