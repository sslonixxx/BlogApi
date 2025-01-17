using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Post")]
public class Post
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

    public List<User> LikedUsers { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int CommentsCount { get; set; } = 0;

    public List<Tag?> Tags { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();

}