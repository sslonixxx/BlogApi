using System.ComponentModel.DataAnnotations;

public class PostFullDto
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

    public string Decription { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int ReadingTime { get; set; }

    public string? Image { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid AuthorId { get; set; }


    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.AuthorLengthError)]

    public string Author { get; set; }

    public Guid? CommunityId { get; set; }

    public string? CommunityName { get; set; }

    public Guid? AdreddId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int Likes { get; set; } = 0;

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public bool HasLike { get; set; } = false;

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int CommentsCount { get; set; } = 0;

    public List<TagDto?> Tags { get; set; } = new();

    public List<CommentDto> Comments { get; set; } = new();

}