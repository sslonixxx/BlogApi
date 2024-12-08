using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

public class AddressController(IAddressService addressService) : ControllerBase
{
    [HttpGet]
    [Route("address/search")]
    public async Task<IActionResult> SearchAddress([FromQuery] long parentObjectId = 0, [FromQuery] string query = "")
    {
        var result = await addressService.SearchAddress(parentObjectId, query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("address/chain")]
    public async Task<IActionResult> SearchChain([FromQuery] Guid objectId)
    {
        var result = await addressService.SearchChain(objectId);
        return Ok(result);
    }
}