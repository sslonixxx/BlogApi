using System.ComponentModel.DataAnnotations;

public class PostDto
{
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public DateTime CreateTime { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.TitleLengthError1)]
    public string Title { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.DescriptionLengthError)]

    public string Description { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int ReadingTime { get; set; }
    
    [RegularExpression(@"^(http|https|ftp)://[^\s/$.?#].[^\s]*$", ErrorMessage = "The Image field is not a valid fully-qualified http, https, or ftp URL.")]
    public string? Image { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid AuthorId { get; set; }


    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.AuthorLengthError)]

    public string Author { get; set; }

    public Guid? CommunityId { get; set; }

    public string? CommunityName { get; set; }

    public Guid? AddressId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int Likes { get; set; } = 0;

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public bool HasLike { get; set; } = false;

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int CommentsCount { get; set; } = 0;

    public List<TagDto?> Tags { get; set; } = new();

}