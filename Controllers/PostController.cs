using blog_api.Models.Post;
using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

[ApiController]
[Route("api")]
public class PostController(IPostService postService, ITokenService tokenService): BaseController
{
    [HttpPost("post")]
    [Authorize]
    public async Task<ActionResult<Guid>> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        
        var postId = await postService.CreatePost(createPostDto, token, null);
        return Ok(postId);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostFullDto>> GetPost(Guid id)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        var post = await postService.GetPostById(id, token);
        return Ok(post);
    }   
    
    [HttpPost("{id:guid}/like")]
    [Authorize]
    public async Task<IActionResult> LikePost(Guid id) {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        await postService.LikePost(id, token);
        return Ok();

    }
    
    [HttpDelete("{id:guid}/like")]
    [Authorize]
    public async Task<IActionResult> DeleteLike(Guid id) {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        await postService.DeleteLike(id, token);
        return Ok();

    }
}