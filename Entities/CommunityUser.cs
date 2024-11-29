using System.ComponentModel.DataAnnotations;

namespace blog_api.Entities;

public class CommunityUser
{
    [Key]
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]

    public Guid CommunityId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public CommunityRole Role { get; set; }
    public User User { get; set; }
    public Community Community { get; set; }
}