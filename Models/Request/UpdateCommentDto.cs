using System.ComponentModel.DataAnnotations;

public class UpdateCommentDto
{
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.ContentLengthError)]
    public string Content { get; set; }
}