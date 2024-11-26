using blog_api.Exeptions;
using blog_api.Mappers;
using blog_api.Models.Post;
using blog_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog_api.Services.Impls;

public class TagService(DataContext context): ITagService
{
    public async Task<List<TagDto>> GetTags()
    {
        var tags = context.Tags.Select(t => TagMapper.TagToTagDto(t)).ToList();
        return tags;
    }
    public void CheckTags(List<Guid> tags)
    {
        foreach (var tagId in tags)
        {
            if (!context.Tags.Any(t => t.Id == tagId)) throw new CustomException($"Can't find tag with {tagId} id", 400);
        }
    }

    public async Task<List<Tag>> GetTagsById(CreatePostDto createPostDto)
    {
        return await context.Tags.Where(e => createPostDto.Tags.Contains(e.Id)).ToListAsync();
    }
}