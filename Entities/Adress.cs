using System.ComponentModel.DataAnnotations.Schema;
using Org.BouncyCastle.Utilities;

[Table("Adress")]
public class Adress
{
    public int ObjectId { get; set; }

    public Guid ObjectGuid { get; set; }

    public string? Text { get; set; }

    public GarAdressLevel GarAdressLevel { get; set; }

    public string? ObjectLevelText { get; set; }

}