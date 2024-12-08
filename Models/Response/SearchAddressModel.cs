using Org.BouncyCastle.Utilities;

public class SearchAddressModel
{
    public long ObjectId { get; set; }

    public Guid ObjectGuid { get; set; }

    public string? Text { get; set; }

    public string Level { get; set; }

    public string? ObjectLevelText { get; set; }

}