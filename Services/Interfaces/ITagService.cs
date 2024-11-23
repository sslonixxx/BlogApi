namespace blog_api.Services.Interfaces;

public interface ITagService
{
    public Task<List<TagDto>> GetTags();
}