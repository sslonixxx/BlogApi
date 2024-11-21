using System.ComponentModel.DataAnnotations;

public record TokenResponse
{
    [Required(ErrorMessage = ErrorConstants.TokenError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.TokenLengthError)]
    public string Token { get; set; }

}