using blog_api.Exeptions;
using blog_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog_api.Services.Impls;

public class CommentService(IAccountService accountService, DataContext context, IPostService postService): ICommentService
{
    
    public async Task CreateComment(CreateCommentDto createCommentDto, Guid postId, string userId)
    {
        var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("Can't find this post", 400);
        var user = await accountService.GetUserById(userId);
        postService.CheckClosedCommunity(post, user);
      
        if (user==null) throw new CustomException("User not found", 404);
        if (createCommentDto.ParentId == null || createCommentDto.ParentId == Guid.Empty)
        {
            await CreateRootComment(createCommentDto, postId, user);
        }
        else
        {
            await CreateChildComment(createCommentDto, postId, user);
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

    public async Task CreateChildComment(CreateCommentDto createCommentDto, Guid postId, User user)
    {
        var parentComment = context.Comments
            .FirstOrDefault(c => c.Id == createCommentDto.ParentId);
        if (parentComment.Content == string.Empty) throw new CustomException("Parent comment is deleted", 404);

        if (parentComment == null)
        {
            throw new CustomException("Parent comment not found", 404);
        }
        else
        {
            parentComment.SubComments++;
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
                CommentParent = parentComment
            };
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }
        
    }
    public async Task EditComment(UpdateCommentDto updateCommentDto, Guid commentId, string userId)
    {
        var user = await accountService.GetUserById(userId);
        var comment = await context.Comments.FindAsync(commentId);
        if (comment == null) throw new CustomException("Comment not found", 404);
        if (user.Id != comment.AuthorId) throw new CustomException("You are not the author of this comment", 403);
        if (comment.Content == string.Empty) throw new CustomException("Comment is already deleted", 404);
        comment.Content = updateCommentDto.Content;
        comment.ModifiedDate = DateTime.UtcNow;
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid commentId, string userId)
    {
        var user = await accountService.GetUserById(userId);
        var comment = await context.Comments.FindAsync(commentId);
        if (comment == null) throw new CustomException("Comment not found", 404);
        if (user.Id != comment.AuthorId) throw new CustomException("You are not the author of this comment", 403);
        if (comment.CommentParent != null)
        {
            
            await DeleteChildComment(comment, user);
        }
        else
        {
            await DeleteRouteComment(comment, user);
        }
    }

    public async Task DeleteRouteComment(Comment comment, User user)
    {
        if (comment.SubComments > 0)
        {
            comment.DeleteDate = DateTime.UtcNow;
            comment.Content = string.Empty;
        }
        else
        {
            context.Remove(comment);
        }
        await context.SaveChangesAsync();
    }

    public async Task DeleteChildComment(Comment comment, User user)
    {
        var parentComment = context.Comments
            .FirstOrDefault(c => c.Id == comment.CommentParent.Id);
        if (parentComment == null) throw new CustomException("Parent comment not found", 404);
        if (comment.SubComments > 0)
        {
            comment.DeleteDate = DateTime.UtcNow;
            comment.Content = string.Empty;
        }
        else
        {
            context.Remove(comment);
        }
        parentComment.SubComments--;
        await context.SaveChangesAsync();
    }

    public async Task<List<CommentDto>> GetCommentTree(Guid commentId, string userId, List<CommentDto>? commentsList = null, bool isRoot = true)
    {
        commentsList ??= new List<CommentDto>();
        var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == comment.PostId);
        var user = await accountService.GetUserById(userId);
        postService.CheckClosedCommunity(post, user);
        if (comment == null)
        {
            return commentsList;
        }
        
        var commentDto = new CommentDto
        {
            Id = comment.Id,
            ModifiedDate = comment.ModifiedDate,
            Content = comment.Content,
            DeleteDate = comment.DeleteDate,
            CreateTime = comment.CreateTime,
            Author = comment.Author,
            AuthorId = comment.AuthorId,
            SubComments = comment.SubComments
        };

        commentsList.Add(commentDto);
        
        var subComments = await context.Comments
            .Where(c => c.CommentParent != null && c.CommentParent.Id == commentId)
            .ToListAsync();

        foreach (var subComment in subComments)
        {
            await GetCommentTree(subComment.Id, userId, commentsList, false);
        }
        if (isRoot && commentsList.Any())
        {
            commentsList.RemoveAt(0);
        }
        return commentsList;
    }


}