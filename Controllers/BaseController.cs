using Microsoft.AspNetCore.Mvc;

namespace blog_api.Controllers;

public class BaseController: ControllerBase
{
    protected Guid GetUserId()
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

        if (userIdClaim == null) throw new UnauthorizedAccessException();

        if (!Guid.TryParse(userIdClaim.Value, out var userId)) throw new InvalidOperationException();
        return userId;
    }
}