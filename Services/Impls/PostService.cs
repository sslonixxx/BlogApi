using blog_api.Exeptions;
using blog_api.Mappers;
using blog_api.Models.Post;
using blog_api.Services.Interfaces;

namespace blog_api.Services.Impls;

public class PostService(DataContext context, IAccountService accountService, ICommunityService communityService, ITagService tagService): IPostService
{

    public async Task<Guid> CreatePost(CreatePostDto createPostDto, string token, Guid? communityId)
    {
        if (createPostDto.Tags.Count == 0) throw new CustomException("Specify at least one tag for a new post",400);
        var posts = context.Posts;
        var user = await accountService.GetUserByToken(token);
        var tags = await tagService.GetTagsById(createPostDto);
        
        var post = PostMapper.MapCreatePostDtoToPost(createPostDto, user, tags);
        posts.Add(post);
        await context.SaveChangesAsync();
        return post.Id;

    }
}