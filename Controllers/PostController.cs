using System.Security.Claims;
using blog_api.Exeptions;
using blog_api.Models.Post;
using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

[ApiController]
[Route("api")]
public class PostController(IPostService postService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("post")]
    [Authorize]
    public async Task<ActionResult<Guid>> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);

        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;

        var postId = await postService.CreatePost(createPostDto, userId, null);
        return Ok(postId);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostFullDto>> GetPost(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);

        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;

        var post = await postService.GetPostById(id, userId);
        return Ok(post);
    }

    [HttpPost("{id:guid}/like")]
    [Authorize]
    public async Task<IActionResult> LikePost(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        await postService.LikePost(id, userId);
        return Ok();
    }

    [HttpDelete("{id:guid}/like")]
    [Authorize]
    public async Task<IActionResult> DeleteLike(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        await postService.DeleteLike(id, userId);
        return Ok();
    }

    [HttpGet("post")]
    [Authorize]
    public async Task<IActionResult> GetPosts([FromQuery] List<Guid>? tags, [FromQuery] string? author,
        [FromQuery] int? minReadingTime, [FromQuery] int? maxReadingTime, [FromQuery] PostSorting? sorting = null,
        [FromQuery] bool onlyMyCommunities = false, [FromQuery] int page = 1, [FromQuery] int size = 5)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        var posts = await postService.GetPosts(tags, author, minReadingTime, maxReadingTime, sorting, onlyMyCommunities,
            page,
            size, userId!);
        return Ok(posts);
    }
}