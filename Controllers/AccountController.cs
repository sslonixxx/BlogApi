using Azure;
using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/account")]
public class AccountController(IAccountService accountService, ILogger<AccountController> logger, ITokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
    {
        var token = await accountService.Register(userRegisterModel);
        return Ok(token);
    }

    [HttpPost("login")]
   
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
    public async Task<IActionResult> Logout()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        
        return Ok(await accountService.Logout(token));
    }

    

}