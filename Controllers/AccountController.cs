using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/account")]
public class AccountController(IAccountService accountService, ILogger<AccountController> logger) : ControllerBase
{
    [HttpPost("register")]
    //[SwaggerOperation(Summary = SwaggerOperationConstants.UserRegister)]
    public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
    {
        logger.LogInformation("reg");
        var token = await accountService.Register(userRegisterModel);
        return Ok(token);
    }

    [HttpPost("login")]
    //[SwaggerOperation(Summary = SwaggerOperationConstants.UserLogin)]
    public async Task<IActionResult> Login(LoginCredentials loginCredentials)
    {
        var token = await accountService.Login(loginCredentials);
        return Ok(token);

    }
    
    [HttpPost("logout")]
    //[SwaggerOperation(Summary = SwaggerOperationConstants.UserLogout)]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        return Ok(await accountService.Logout());
    }
    
    

}