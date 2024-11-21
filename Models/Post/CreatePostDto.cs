using System.ComponentModel.DataAnnotations;

public class CreatePostDto
{

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 5, ErrorMessage = ErrorConstants.TitleLengthError5)]
    public string Title { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.DescriptionLengthError)]

    public string Decription { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int ReadingTime { get; set; }

    public string? Image { get; set; }

    public Guid? AdreddId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.TagsLengthError)]
    public List<Guid> Tags { get; set; } = new();


}