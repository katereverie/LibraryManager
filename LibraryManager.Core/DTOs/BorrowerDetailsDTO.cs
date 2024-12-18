﻿namespace LibraryManager.Core.DTOs
{
    public class BorrowerDetailsDTO
    {
        public int BorrowerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<CheckoutLogDTO>? CheckoutLogs { get; set; }
    }

    public class CheckoutLogDTO
    {
        public int CheckoutLogID { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int MediaID { get; set; }
        public string Title { get; set; }
        public string MediaTypeName { get; set; }
    }
}
