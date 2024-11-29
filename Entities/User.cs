using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using blog_api.Entities;

[Table("User")]
public class User
{
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [Key] public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public DateTime CreateTime { get; set; } = DateTime.Now;

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.NameLengthError)]
    public string Name { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public GenderEnum Gender { get; set; }

    [EmailAddress(ErrorMessage = ErrorConstants.EmailValidError)]
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.EmailLengthError)]
    public string Email { get; set; }

    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 6, ErrorMessage = ErrorConstants.PasswordLengthError)]
    public string Password { get; set; }

    public List<Post>? LikedPosts { get; set; } = new();
    
    public List<CommunityUser>? CommunityUser { get; set; } = new();

}