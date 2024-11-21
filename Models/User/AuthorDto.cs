using System.ComponentModel.DataAnnotations;

public class AuthorDto
{
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public DateTime CreateTime { get; set; }
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.NameLengthError)]
    public string Name { get; set; }

    public string? BirthDate { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public GenderEnum Gender { get; set; }

    public int Posts { get; set; }

    public int Likes { get; set; }

    public DateTime Created { get; set; }

}