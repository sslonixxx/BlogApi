using System.Security.Claims;
using blog_api.Exeptions;
using blog_api.Models.Post;
using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

[ApiController]
[Route("api")]
public class CommentController(ITokenService tokenService, ICommentService commentService): ControllerBase
{
    [HttpPost("post/{postId}/comment")]
    [Authorize]
    public async Task<IActionResult> CreateComment([FromRoute] Guid postId, [FromBody] CreateCommentDto comment)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;

        await commentService.CreateComment(comment, postId, userId);
        return Ok();

    }

    [HttpPut("comment/{commentId}")]
    [Authorize]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid commentId, [FromBody] UpdateCommentDto comment)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        
        await commentService.EditComment(comment, commentId, userId);
        return Ok();
    }

    [HttpDelete("comment/{commentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        
        await commentService.DeleteComment(commentId, userId);
        return Ok();
    }

    [HttpGet("comment/{commentId}/tree")]
    public async Task<IActionResult> GetCommentTree([FromRoute] Guid commentId)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        
        var comments = await commentService.GetCommentTree(commentId, userId);
        return Ok(comments);
    }
}