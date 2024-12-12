namespace blog_api.Mappers;

public static class CommentMapper
{
    public static CommentDto CommentToCommentDto(Comment comment)
    {
        var commentDto = new CommentDto()
        {
            Id = comment.Id,
            Content = comment.Content,
            CreateTime = comment.CreateTime,
            ModifiedDate = comment.ModifiedDate,
            DeleteDate = comment.DeleteDate,
            AuthorId = comment.AuthorId,
            Author = comment.Author,
            SubComments = comment.SubComments

        };
        return commentDto;
    }
}