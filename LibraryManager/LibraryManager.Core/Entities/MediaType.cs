namespace LibraryManager.Core.Entities
{
    public class MediaType
    {
        public int MediaTypeID { get; set; }
        public required string MediaTypeName { get; set; }

        public List<Media>? Medias { get; set; }
    }
}
