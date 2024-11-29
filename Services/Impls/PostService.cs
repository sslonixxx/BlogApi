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
            .Include(post => post.CommunityId)
            .Include(post => post.CommunityName)
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
        
        var community = context.Communities.FirstOrDefault(community => community.Id == communityId);
        string communityName = community?.Name;
        if (community != null && context.CommunityUser.All(uc => uc.UserId != user.Id || uc.Role!=CommunityRole.Administrator))
            throw new CustomException("User is not an admin of this community", 403);
        var post = PostMapper.MapCreatePostDtoToPost(createPostDto, user, tags, communityId, communityName);
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
        return PostMapper.PostToPostFullDto(post, user, tags);
    }

    public void CheckClosedCommunity(Post post, User? user)
    {
        Community? community = context.Communities.FirstOrDefault(c => c.Id == post.CommunityId);
        if (post.CommunityId != null && community.IsClosed &&
            (user == null || user.CommunityUser.All(uc => uc.CommunityId != community.Id )))
            throw new CustomException("You don't have rights", 403);
    }
    public async Task LikePost(Guid postId, string token)
    {
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var post = await context.Posts.Include(p => p.LikedUsers).Include(p => p.CommunityId).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await accountService.GetUserByToken(token);
        CheckClosedCommunity(post, user);
        if (post.LikedUsers.Any(u => u.Id == user.Id)) throw new CustomException("User has already liked post", 400);
        Community? community = context.Communities.FirstOrDefault(c => c.Id == post.CommunityId);
        if(post.CommunityId != null && community.IsClosed && user.CommunityUser.All(c => c.UserId!=user.Id)) 
            throw new CustomException("User can't add like to this post", 403);
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
        CheckClosedCommunity(post, user);
        if (post.LikedUsers.Any(u => u.Id != user.Id)) throw new CustomException("User didn't like this post", 400);
        post.LikedUsers?.Remove(user);
        user.LikedPosts?.Remove(post);
        post.Likes -= 1;
        await context.SaveChangesAsync();
    }
}