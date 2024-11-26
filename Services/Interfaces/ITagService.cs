using blog_api.Models.Post;

namespace blog_api.Services.Interfaces;

public interface ITagService
{
    public Task<List<TagDto>> GetTags();
    public Task<List<Tag>> GetTagsById(CreatePostDto createPostDto);
}