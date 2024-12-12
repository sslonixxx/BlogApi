using System.ComponentModel.DataAnnotations;
using blog_api.Constants;

public class UserDto
{

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public DateTime CreateTime { get; set; }
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.NameLengthError)]
    public string Name { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public GenderEnum Gender { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.EmailLengthError)]
    [EmailAddress(ErrorMessage = ErrorConstants.EmailNotValid)]
    public string Email { get; set; }

    [RegularExpression(RegexConstants.PhoneNumberRegex)]
    [MinAge(14, ErrorMessage = "The user must be at least 14 years old.")]
    public string? PhoneNumber { get; set; }

}
