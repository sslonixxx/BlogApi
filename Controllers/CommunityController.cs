using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

[Route("api/community")]
public class CommunityController(ICommunityService communityService): ControllerBase
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
    
}