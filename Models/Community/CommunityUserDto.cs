using System.ComponentModel.DataAnnotations;

public class CommunityUserDto
{
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]

    public Guid CommunityId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public CommunityRole Role { get; set; }
}