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
        return Ok(await communityService.GetCommunityInformation(id));
    }

    [HttpPost("{id:guid}/post")]
    [Authorize]
    public async Task<IActionResult> CreatePostInCommunity(Guid id, [FromBody]CreatePostDto createPostDto)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        var postId = await postService.CreatePost(createPostDto, token, id);
        return Ok(postId);
    }

    [HttpGet("{id:guid}/role")]
    [Authorize]
    public async Task<ActionResult<string>> GetUserRole(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        var communityRole = await communityService.GetUserRole(id, token);
        if (communityRole == null) return Ok("null");
        return Ok(communityRole);
    }

    [HttpPost("{id:guid}/subscribe")]
    [Authorize]
    public async Task<IActionResult> SubscribeCommunuty(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        await communityService.SubscribeCommunity(id, token);
        return Ok();

    }

    [HttpDelete("{id:guid}/unsubscribe")]
    [Authorize]
    public async Task<IActionResult> UnsubscribeCommunuty(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        await communityService.UnsubscribeCommunity(id, token);
        return Ok();
    }

    // [HttpGet("my")]
    // [Authorize]
    // public async Task<IActionResult> GetMyCommunities()
    // {
    //     var authorizationHeader = Request.Headers["Authorization"].ToString();
    //     var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
    //     var myCommunities = await communityService.GetMyCommunities(token);
    //     return Ok(myCommunities);
    // }
    
}