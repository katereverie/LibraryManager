﻿namespace LibraryManager.UI.Models;

public class EditBorrowerRequest
{
    public int BorrowerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
