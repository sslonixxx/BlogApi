using blog_api.Models.Post;

namespace blog_api.Mappers;

public abstract class PostMapper
{
    public static Post MapCreatePostDtoToPost(CreatePostDto createPostDto, User user, List<Tag?> tags)
    {
        var post = new Post()
        {
            Id = Guid.NewGuid(),
            Description = createPostDto.Description,
            CreateTime = DateTime.UtcNow,
            Author = user.Name,
            AuthorId = user.Id,
            Tags = tags,
            ReadingTime = createPostDto.ReadingTime,
            Title = createPostDto.Title,
            AddressId = createPostDto.AddressId,
            Image = createPostDto.Image
        };
        return post;
    }
}