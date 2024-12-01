namespace blog_api.Services.Interfaces;

public interface ICommentService
{
    public Task CreateComment(CreateCommentDto createCommentDto, Guid postId, string userId);
    public Task CreateRootComment(CreateCommentDto createCommentDto, Guid postId, User user);
    public Task EditComment(UpdateCommentDto updateCommentDto, Guid commentId, string userId);


}