using blog_api.Models.Post;

namespace blog_api.Services.Interfaces;

public interface IPostService
{
    public Task<Guid> CreatePost(CreatePostDto createPostDto, string token, Guid? communityId);
}