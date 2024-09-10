namespace LibraryManager.Core.Entities
{
    public class TopThreeMedia
    {
        public int MediaID { get; set; }
        public required string MediaTypeName { get; set; }
        public required string Title { get; set; }
        public int CheckoutCount { get; set; }
        
    }
}
