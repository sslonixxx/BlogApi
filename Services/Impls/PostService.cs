using blog_api.Exeptions;
using blog_api.Mappers;
using blog_api.Models.Post;
using blog_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;

namespace blog_api.Services.Impls;

public class PostService(
    DataContext context,
    IAccountService accountService,
    ICommunityService communityService,
    ITagService tagService,
    ITokenService tokenService) : IPostService
{
    private IQueryable<Post> GetAllPosts()
    {
        var posts = context.Posts
            .Include(post => post.Tags)
            .Include(post => post.LikedUsers);

        return posts;
    }

    public async Task<Guid> CreatePost(CreatePostDto createPostDto, string userId, Guid? communityId)
    {
        if (createPostDto.Tags.Count == 0) throw new CustomException("Specify at least one tag for a new post", 400);
        var posts = context.Posts;
        var user = await accountService.GetUserById(userId);
        var tags = await tagService.GetTagsById(createPostDto);

        var community = context.Communities.FirstOrDefault(community => community.Id == communityId);
        string communityName = community?.Name;
        if (community != null &&
            context.CommunityUser.All(uc => uc.UserId != user.Id || uc.Role != CommunityRole.Administrator))
            throw new CustomException("User is not an admin of this community", 403);
        var post = PostMapper.MapCreatePostDtoToPost(createPostDto, user, tags, communityId, communityName);
        posts.Add(post);
        await context.SaveChangesAsync();
        return post.Id;
    }

    public async Task<PostFullDto> GetPostById(Guid postId, string userId)
    {
        var post = await GetAllPosts().FirstOrDefaultAsync(p => p.Id == postId);
        //var post = context.Posts.FirstOrDefault(post => post.Id == postId);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var tags = post.Tags.Select(t => TagMapper.TagToTagDto(t)).ToList();
        var user = await accountService.GetUserById(userId);
        //проверить права
        return PostMapper.PostToPostFullDto(post, user, tags);
    }

    public void CheckClosedCommunity(Post post, User? user)
    {
        Community? community = context.Communities.FirstOrDefault(c => c.Id == post.CommunityId);
        if (post.CommunityId != null && community.IsClosed &&
            (user == null || user.CommunityUser.All(uc => uc.CommunityId != community.Id)))
            throw new CustomException("You don't have rights", 403);
    }

    public async Task LikePost(Guid postId, string userId)
    {
        var post = await GetAllPosts().FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        
        var user = await accountService.GetUserById(userId);
        CheckClosedCommunity(post, user);
        if (post.LikedUsers.Any(u => u.Id == user.Id)) throw new CustomException("User has already liked post", 400);
        Community? community = context.Communities.FirstOrDefault(c => c.Id == post.CommunityId);
        if (post.CommunityId != null && community.IsClosed && user.CommunityUser.All(c => c.UserId != user.Id))
            throw new CustomException("User can't add like to this post", 403);
        post.LikedUsers?.Add(user);
        user.LikedPosts?.Add(post);
        post.Likes += 1;
        await context.SaveChangesAsync();
    }

    public async Task DeleteLike(Guid postId, string userId)
    {
        var post = await context.Posts.Include(p => p.LikedUsers).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await accountService.GetUserById(userId);
        CheckClosedCommunity(post, user);
        if (post.LikedUsers.Any(u => u.Id != user.Id)) throw new CustomException("User didn't like this post", 400);
        post.LikedUsers?.Remove(user);
        user.LikedPosts?.Remove(post);
        post.Likes -= 1;
        await context.SaveChangesAsync();
    }

    public async Task<PostPagedListDto> GetPosts(List<Guid>? tags, string? author, int? minReadingTime,
        int? maxReadingTime, PostSorting? sorting,
        bool onlyMyCommunities, int page, int size, string userId)
    {
        var posts = context.Posts.AsQueryable();
        if (minReadingTime != null && maxReadingTime != null && maxReadingTime < minReadingTime)
            throw new CustomException("Max reading time can't be less than min reading time", 400);
        if (size <= 0) throw new CustomException("Invalid size value", 400);
        if (page <= 0) throw new CustomException("Invalid page value", 400);

        var user = await accountService.GetUserById(userId);
        //var posts = GetAllPosts();
        

        foreach (var tagId in tags)
        {
            if (!context.Tags.Any(t => t.Id == tagId))
                throw new CustomException($"Can't find tag with {tagId} id", 400);
        }

        posts = posts.Where(p => p.Tags!.Any(tag => tags.Contains(tag.Id)));
        if (author != null) posts = posts.Where(p => p.Author == author);
        if (minReadingTime != null) posts = posts.Where(p => p.ReadingTime >= minReadingTime);
        if (maxReadingTime != null) posts = posts.Where(p => p.ReadingTime <= maxReadingTime);

        var closedCommunitiesId = context.Communities.Where(c => c.IsClosed).Select(c => c.Id).ToList();
        var userCommunitiesId =
            context.CommunityUser.Where(c => c.UserId == user.Id).Select(c => c.CommunityId).ToList();
        posts = posts.Where(p =>
            !closedCommunitiesId.Contains(p.Id) || userCommunitiesId.Contains(p.CommunityId.Value));
        ;
        posts = sorting switch
        {
            PostSorting.CreateAsc => posts.OrderBy(p => p.CreateTime),
            PostSorting.CreateDesc => posts.OrderByDescending(p => p.CreateTime),
            PostSorting.LikeAsc => posts.OrderBy(p => p.Likes),
            PostSorting.LikeDesc => posts.OrderByDescending(p => p.Likes),
            _ => throw new CustomException("Invalid sorting value", 400)
        };
        if (onlyMyCommunities)
        {
            var userCommunities = context.Communities.Where(c => c.CommunityUser.Any(u => u.UserId == user.Id));
            posts = posts.Where(p => userCommunities.Any(c => c.Id == p.CommunityId));
        }

        var paginatedData = posts.Skip((page - 1) * size).Take(size).ToList();
        var postsFull = new List<PostDto>();
        foreach (var post in paginatedData)
        {
            var totalPost = await GetTotalPost(post.Id, user.Id);
            if (totalPost != null) postsFull.Add(totalPost);
        }

        var totalCount = posts.Count();

        var pagination = new PageInfoModel()
        {
            Count = (int)Math.Ceiling((decimal)totalCount / size),
            Size = size,
            Current = page
        };
        
        return PostMapper.ToPostPagedListDto(postsFull, pagination);
    }

    public async Task<PostDto?> GetTotalPost(Guid postId, Guid userId)
    {
        var post = context.Posts.Include(p => p.Tags)
            .Include(p => p.LikedUsers)
            .FirstOrDefault(p => p.Id == postId);
        if (post == null) return null;
        //комментарии
        var postDto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Likes = post.Likes,
            Image = post.Image,
            Author = post.Author,
            AuthorId = post.AuthorId,
            CommunityId = post.CommunityId,
            CommunityName = post.CommunityName,
            AddressId = post.AddressId,
            CommentsCount = 0,
            HasLike = post.LikedUsers.Any(u => u.Id == userId),
            Tags = post.Tags.Select(t => TagMapper.TagToTagDto(t)).ToList(),
        };
        return postDto;
    }

    public async Task<PostPagedListDto> GetCommunityPosts(Guid communityId, List<Guid>? tags, PostSorting? sorting,
        int page, int size, string userId)
    {
        var user = await accountService.GetUserById(userId);
        if (page <= 0) throw new CustomException("Invalid page value", 400);
        var community = context.Communities.FirstOrDefault(c => c.Id == communityId);
        if (community == null) throw new CustomException("Community not found", 400);
        if (community.IsClosed && community.CommunityUser.All(c => c.UserId != user.Id))
        {
            throw new CustomException("Can't see community's posts", 400);
        }

        var posts = GetAllPosts();
        posts = posts.Where(p => p.CommunityId == communityId);
        foreach (var tagId in tags)
        {
            if (!context.Tags.Any(t => t.Id == tagId))
                throw new CustomException($"Can't find tag with {tagId} id", 400);
        }

        posts = posts.Where(p => p.Tags!.Any(tag => tags.Contains(tag.Id)));
        posts = sorting switch
        {
            PostSorting.CreateAsc => posts.OrderBy(p => p.CreateTime),
            PostSorting.CreateDesc => posts.OrderByDescending(p => p.CreateTime),
            PostSorting.LikeAsc => posts.OrderBy(p => p.Likes),
            PostSorting.LikeDesc => posts.OrderByDescending(p => p.Likes),
            _ => throw new CustomException("Invalid sorting value", 400)
        };

        var paginatedData = posts.Skip((page - 1) * size).Take(size).ToList();
        var postsFull = new List<PostDto>();
        foreach (var post in paginatedData)
        {
            var totalPost = await GetTotalPost(post.Id, user.Id);
            if (totalPost != null) postsFull.Add(totalPost);
        }

        var totalCount = posts.Count();

        var pagination = new PageInfoModel()
        {
            Count = (int)Math.Ceiling((decimal)totalCount / size),
            Size = size,
            Current = page
        };

        return PostMapper.ToPostPagedListDto(postsFull, pagination);
    }
}