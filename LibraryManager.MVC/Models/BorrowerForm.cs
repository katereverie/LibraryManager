using LibraryManager.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.MVC.Models;

public class BorrowerForm
{
    public int? BorrowerID { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 1,
        ErrorMessage = "First name must be between {2} and {1} characters")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\u4E00-\u9FFF\u3400-\u4DBF\u3040-\u309F\u30A0-\u30FF\s-']+$",
        ErrorMessage = "First name can only contain letters, spaces, hyphens, apostrophes, and common Asian characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 1,
        ErrorMessage = "Last name must be between {2} and {1} characters")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\u4E00-\u9FFF\u3400-\u4DBF\u3040-\u309F\u30A0-\u30FF\s-']+$",
        ErrorMessage = "Last name can only contain letters, spaces, hyphens, apostrophes, and common Asian characters")]
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
