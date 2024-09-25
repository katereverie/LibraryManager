using System.ComponentModel.DataAnnotations;

namespace LibraryManager.API.Models
{
    public class EditMedia
    {
        public int MediaID { get; set; }
        public int MediaTypeID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title field musn't be empty.")]
        public string Title { get; set; }
        public bool IsArchived { get; set; }

    }
}
