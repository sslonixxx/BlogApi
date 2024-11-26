// using blog_api.Services.Interfaces;
// using Microsoft.AspNetCore.Mvc;
//
// namespace blog_api.Controllers;
//
// [Route("api/community")]
// public class CommunityController(ICommunityService communityService): ControllerBase
// {
//     [HttpGet("")]
//     public async Task<IActionResult> GetCommunity()
//     {
//         var community = communityService.GetCommunity();
//         return Ok(community);
//     }
//     
// }