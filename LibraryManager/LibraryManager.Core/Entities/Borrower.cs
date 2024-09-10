namespace LibraryManager.Core.Entities
{
    public class Borrower
    {
        public int BorrowerID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        public List<CheckoutLog>? CheckoutLogs { get; set; }
    }
}
