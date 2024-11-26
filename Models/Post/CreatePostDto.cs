using System.ComponentModel.DataAnnotations;

namespace blog_api.Models.Post;

public class CreatePostDto
{

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 5, ErrorMessage = ErrorConstants.TitleLengthError5)]
    public string Title { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.DescriptionLengthError)]

    public string Description { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int ReadingTime { get; set; }

    public string? Image { get; set; }

    public Guid? AddressId { get; set; } = null;

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [MinElements(1, ErrorMessage = "The Tags list must contain at least one element.")]
    public List<Guid> Tags { get; set; } = new();


}