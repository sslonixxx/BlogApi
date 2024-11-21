using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Community")]
public class Community
{

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public DateTime CreateTime { get; set; }
    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorConstants.NameLengthError)]
    public string Name { get; set; }

    public string? Decription { get; set; }

    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public bool IsClosed { get; set; } = false;


    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public int SubscribersCount { get; set; } = 0;


    [Required(ErrorMessage = ErrorConstants.RequiredFieldError)]
    public List<UserDto> Administrators { get; set; } = new();

}