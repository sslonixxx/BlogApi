using blog_api.Models.Post;

namespace blog_api.Services.Interfaces;

public interface IPostService
{
    public Task<Guid> CreatePost(CreatePostDto createPostDto, string token, Guid? communityId);
    public Task<PostFullDto> GetPostById(Guid postId, string token);
    public Task LikePost(Guid postId, string token);
    public Task DeleteLike(Guid postId, string token);

    public Task<PostPagedListDto> GetPosts(List<Guid>? tags, string? author, int? minReadingTime,
        int? maxReadingTime, PostSorting? sorting,
        bool onlyMyCommunities, int page, int size, string userId);
    
    public Task<PostDto?> GetTotalPost(Guid postId, Guid userId);

    public Task<PostPagedListDto> GetCommunityPosts(Guid communityId, List<Guid>? tags, PostSorting? sorting,
        int page, int size, string token);

}