using blog_api.Models.Post;

namespace blog_api.Services.Interfaces;

public interface IPostService
{
    public Task<Guid> CreatePost(CreatePostDto createPostDto, string token, Guid? communityId);
    public Task<PostFullDto> GetPostById(Guid postId, string token);
    public Task LikePost(Guid postId, string token);
    public Task DeleteLike(Guid postId, string token);

}