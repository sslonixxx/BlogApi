using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

[Route("api/tag")]
public class TagController(ITagService tagService): ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetTags()
    {
        var tags = await tagService.GetTags();
        return Ok(tags);
    }
    
}