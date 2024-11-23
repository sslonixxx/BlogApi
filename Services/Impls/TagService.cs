using blog_api.Mappers;
using blog_api.Services.Interfaces;

namespace blog_api.Services.Impls;

public class TagService(DataContext context): ITagService
{
    public async Task<List<TagDto>> GetTags()
    {
        var tags = context.Tags.Select(t => TagMapper.TagToTagDto(t)).ToList();
        return tags;
    }
}