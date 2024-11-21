using System.ComponentModel.DataAnnotations;

public record LoginCredentials
{
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 6, ErrorMessage = ErrorConstants.PasswordLengthError)]
    public string Password { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.EmailLengthError)]
    public string Email { get; set; }


}