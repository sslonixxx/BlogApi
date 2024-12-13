using System.Security.Claims;
using blog_api.Exeptions;
using blog_api.Models.Post;
using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

[Route("api/community")]
public class CommunityController(ICommunityService communityService, IPostService postService, ITokenService tokenService): ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetCommunities()
    {
        var community = communityService.GetCommunities();
        return Ok(community);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCommunityInformation(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        return Ok(await communityService.GetCommunityInformation(id, userId));
    }

    [HttpPost("{id:guid}/post")]
    [Authorize]
    public async Task<IActionResult> CreatePostInCommunity(Guid id, [FromBody]CreatePostDto createPostDto)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        var postId = await postService.CreatePost(createPostDto, userId, id);
        return Ok(postId);
    }

    [HttpGet("{id:guid}/role")]
    [Authorize]
    public async Task<ActionResult<string>> GetUserRole(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        var communityRole = await communityService.GetUserRole(id, userId);
        if (communityRole == null) return Ok("null");
        return Ok(communityRole);
    }

    [HttpPost("{id:guid}/subscribe")]
    [Authorize]
    public async Task<IActionResult> SubscribeCommunuty(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        await communityService.SubscribeCommunity(id, userId);
        return Ok();

    }

    [HttpDelete("{id:guid}/unsubscribe")]
    [Authorize]
    public async Task<IActionResult> UnsubscribeCommunuty(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        await communityService.UnsubscribeCommunity(id, userId);
        return Ok();
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyCommunities()
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        var myCommunities = await communityService.GetMyCommunities(userId);
        return Ok(myCommunities);
    }

    [HttpGet("{id:guid}/post")]
    [Authorize]
    public async Task<IActionResult> GetCommunityPosts(Guid id, [FromQuery] List<Guid>? tags,
        [FromQuery] PostSorting? sorting, [FromQuery] int page=1, [FromQuery] int size=5 )
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        var posts = await postService.GetCommunityPosts(id, tags, sorting, page, size, userId);
        return Ok(posts);
    }
    
}