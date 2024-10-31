using LibraryManager.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.MVC.Models;

public class BorrowerForm
{
    public int? BorrowerID { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "First name must be between {2} and {1} characters")]
    [RegularExpression(@"^[A-Za-z\s-']+$", 
        ErrorMessage = "First name can only contain letters, spaces, hyphens and apostrophes")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "Last name must be between {2} and {1} characters")]
    [RegularExpression(@"^[A-Za-z\s-']+$",
        ErrorMessage = "Last name can only contain letters, spaces, hyphens and apostrophes")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100, ErrorMessage = "Email cannot exceed {1} characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", 
        ErrorMessage = "Please enter a valid phone number")]
    [DataType(DataType.PhoneNumber)]
    [StringLength(20, MinimumLength = 10, 
        ErrorMessage = "Phone number must be between {2} and {1} digits")]
    public string Phone { get; set; } = string.Empty;

    public BorrowerForm() { }

    public BorrowerForm(Borrower entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        BorrowerID = entity.BorrowerID;
        FirstName = entity.FirstName;
        LastName = entity.LastName;
        Email = entity.Email;
        Phone = entity.Phone;
    }

    public Borrower ToEntity()
    {
        return new Borrower
        {
            BorrowerID = BorrowerID ?? 0,
            FirstName = FirstName.Trim(),
            LastName = LastName.Trim(),
            Email = Email.Trim().ToLowerInvariant(), // culture-independent
            Phone = Phone.Trim(),
        };
    }
}
