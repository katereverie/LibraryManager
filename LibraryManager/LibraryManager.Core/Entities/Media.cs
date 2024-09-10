namespace LibraryManager.Core.Entities
{
    public class Media
    {
        public int MediaID { get; set; }
        public int MediaTypeID { get; set; }
        public required string Title { get; set; }
        public bool IsArchived { get; set; }

        public MediaType MediaType { get; set; }
        public List<CheckoutLog>? CheckoutLogs { get; set; }
    }
}
