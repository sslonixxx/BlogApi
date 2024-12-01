using blog_api.Models.Post;

namespace blog_api.Mappers;

public abstract class PostMapper(DataContext context)
{
    public static Post MapCreatePostDtoToPost(CreatePostDto createPostDto, User user, List<Tag?> tags, Guid? communityId, string communityName)
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
            Image = createPostDto.Image,
            CommunityId = communityId,
            CommunityName = communityName
        };
        return post;
    }

    public static PostFullDto PostToPostFullDto(Post post, User user, List<TagDto?> tags)
    {
        var postFullDto = new PostFullDto()
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            AuthorId = post.AuthorId,
            Author = user.Name,
            ReadingTime = post.ReadingTime,
            CreateTime = post.CreateTime,
            AddressId = post.AddressId,
            Image = post.Image,
            CommunityId = post.CommunityId,
            CommunityName = post.CommunityName,
            Likes = post.Likes,
            HasLike = post.LikedUsers.Any(u => u.Id == post.AuthorId),
            //добавить комментарии и cкушать тортик))))))))
            Tags = tags
        };
        return postFullDto;
    }

    public static PostPagedListDto ToPostPagedListDto(List<PostDto?> postsFull, PageInfoModel pagination)
    {
        return new PostPagedListDto()
        {
            PostDto = postsFull,
            PageInfoModel = pagination
        };
    }
}