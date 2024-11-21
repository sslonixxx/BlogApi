using Org.BouncyCastle.Utilities;

public class SearchAdressModel
{
    public int ObjectId { get; set; }

    public Guid ObjectGuid { get; set; }

    public string? Text { get; set; }

    public GarAdressLevel GarAdressLevel { get; set; }

    public string? ObjectLevelText { get; set; }

}