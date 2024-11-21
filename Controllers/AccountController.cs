using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/account")]
public class AccountController(IAccountService accountService, ILogger<AccountController> logger) : ControllerBase
{
    [HttpGet]
public IActionResult Get()
{
    return Ok();
}


    [HttpPost("register")]
    //[SwaggerOperation(Summary = SwaggerOperationConstants.UserRegister)]
    public async Task<IResult> Register(UserRegisterModel userRegisterModel)
    {
        logger.LogInformation("reg");
        var token = await accountService.Register(userRegisterModel);
        return Results.Ok(token);
    }

    [HttpPost("login")]
    //[SwaggerOperation(Summary = SwaggerOperationConstants.UserLogin)]
    public async Task<IResult> Login(LoginCredentials loginCredentials)
    {
        var token = await accountService.Login(loginCredentials);
        return Results.Ok(token);

    }
}