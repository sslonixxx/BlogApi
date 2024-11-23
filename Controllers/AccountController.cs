using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/account")]
public class AccountController(IAccountService accountService, ILogger<AccountController> logger, ITokenService tokenService) : ControllerBase
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

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
    
        return Ok(await accountService.GetProfile(token));
    }
    
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> EditProfile(UserEditModel userEditModel)
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var token = tokenService.ExtractTokenFromHeader(authorizationHeader);
        return Ok(await accountService.EditProfile(userEditModel, token));
    }
    
    [Authorize]
    [HttpPost("logout")]
    //[SwaggerOperation(Summary = SwaggerOperationConstants.UserLogout)]
    public async Task<IActionResult> Logout()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        
        return Ok(await accountService.Logout(token));
    }

    

}