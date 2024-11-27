using blog_api.Exeptions;
using blog_api.Mappers;
using blog_api.Models.Post;
using blog_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog_api.Services.Impls;

public class PostService(DataContext context, IAccountService accountService, ICommunityService communityService, ITagService tagService, ITokenService tokenService): IPostService
{
    
    private  IQueryable<Post> GetAllPosts()
    {
        var posts = context.Posts
            .Include(post => post.Tags)
            .Include(post => post.LikedUsers);
           
        return posts;
    }

    public async Task<Guid> CreatePost(CreatePostDto createPostDto, string token, Guid? communityId)
    {
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        if (createPostDto.Tags.Count == 0) throw new CustomException("Specify at least one tag for a new post",400);
        var posts = context.Posts;
        var user = await accountService.GetUserByToken(token);
        var tags = await tagService.GetTagsById(createPostDto);
        
        var post = PostMapper.MapCreatePostDtoToPost(createPostDto, user, tags);
        posts.Add(post);
        await context.SaveChangesAsync();
        return post.Id;

    }

    public async Task<PostFullDto> GetPostById(Guid postId, string token) 
    {
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var post = await GetAllPosts().FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var tags = post.Tags.Select(t => TagMapper.TagToTagDto(t)).ToList();
        User? user = null;
        if(tokenService.ValidateToken(token)) user = await accountService.GetUserByToken(token);
        //проверить права
        Console.WriteLine(post.LikedUsers);
        return PostMapper.PostToPostFullDto(post, user, tags);
    }

    public async Task LikePost(Guid postId, string token)
    {
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var post = await context.Posts.Include(p => p.LikedUsers).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await accountService.GetUserByToken(token);
        //проверить права
        if (post.LikedUsers.Any(u => u.Id == user.Id)) throw new CustomException("User has already liked post", 400);
        post.LikedUsers?.Add(user);
        user.LikedPosts?.Add(post);
        post.Likes += 1;
        await context.SaveChangesAsync();
    }

    public async Task DeleteLike(Guid postId, string token)
    {
        if(await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var post = await context.Posts.Include(p => p.LikedUsers).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await accountService.GetUserByToken(token);
        //проверить права
        if (post.LikedUsers.Any(u => u.Id != user.Id)) throw new CustomException("User didn't like this post", 400);
        post.LikedUsers?.Remove(user);
        user.LikedPosts?.Remove(post);
        post.Likes -= 1;
        await context.SaveChangesAsync();
    }
}