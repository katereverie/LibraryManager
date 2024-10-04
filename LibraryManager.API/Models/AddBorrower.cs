using System.ComponentModel.DataAnnotations;

namespace LibraryManager.API.Models
{
    public class AddBorrower : IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address format.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number format.")]
        public string Phone {  get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName.Any(char.IsDigit) || FirstName.Any(char.IsWhiteSpace))
            {
                yield return new ValidationResult("First name should not contain digits or spaces.");
            } 

            if (LastName.Any(char.IsDigit) || LastName.Any(char.IsWhiteSpace))
            {
                yield return new ValidationResult("Last name should not contain digits or spaces.");
            }
        }

    }
}
