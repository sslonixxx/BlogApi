using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum PostSorting
{
    CreateDesc, CreateAsc, LikeAsc, LikeDesc
}