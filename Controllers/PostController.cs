using blog_api.Models.Post;
using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

[Microsoft.AspNetCore.Components.Route("api/post")]
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

}