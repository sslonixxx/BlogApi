namespace blog_api.Mappers;

public class TagMapper
{
    public static TagDto TagToTagDto(Tag tag)
    {
        var tagDto = new TagDto()
        {
            Id = tag.Id,
            CreateTime = tag.CreateTime,
            Name = tag.Name
        };
        return tagDto;
    }
}