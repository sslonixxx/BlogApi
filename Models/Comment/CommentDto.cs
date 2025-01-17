using System.ComponentModel.DataAnnotations;

public class CommentDto
{
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public DateTime CreateTime { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.ContentLengthError)]
    public string Content { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid AuthorId { get; set; }


    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.AuthorLengthError)]

    public string Author { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int SubComments { get; set; }



}