using System.ComponentModel.DataAnnotations;

namespace blog_api.Entities;

public class CommunityUser
{
    [Key]
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid userId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]

    public Guid CommunityId { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public CommunityRole Role { get; set; }
}