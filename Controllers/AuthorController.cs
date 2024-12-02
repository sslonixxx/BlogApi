using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController(IAuthorService authorService): ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await authorService.GetAuthors();
        return Ok(authors);
    }
}