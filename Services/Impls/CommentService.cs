using blog_api.Exeptions;
using blog_api.Services.Interfaces;

namespace blog_api.Services.Impls;

public class CommentService(IAccountService accountService, DataContext context): ICommentService
{
    public async Task CreateComment(CreateCommentDto createCommentDto, Guid postId, string userId)
    {
        var user = await accountService.GetUserById(userId);
        if (user==null) throw new CustomException("User not found", 404);
        if (createCommentDto.ParentId == null || createCommentDto.ParentId == Guid.Empty)
        {
            await CreateRootComment(createCommentDto, postId, user);
        } 
    }
    public async Task CreateRootComment(CreateCommentDto createCommentDto, Guid postId, User user)
    {
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            PostId = postId,
            Content = createCommentDto.Content,
            CreateTime = DateTime.UtcNow,
            ModifiedDate = null,
            DeleteDate = null,
            User = user,
            Author = user.Name,
            AuthorId = user.Id,
            CommentParent = null
        };
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
    }

    public async Task EditComment(UpdateCommentDto updateCommentDto, Guid commentId, string userId)
    {
        var user = await accountService.GetUserById(userId);
        var comment = await context.Comments.FindAsync(commentId);
        if (comment == null) throw new CustomException("Comment not found", 404);
        if (user.Id != comment.AuthorId) throw new CustomException("You are not the author of this comment", 403);
        if (comment.Content == null) throw new CustomException("Comment is already deleted", 404);
        comment.Content = updateCommentDto.Content;
        comment.ModifiedDate = DateTime.UtcNow;
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
    }
}