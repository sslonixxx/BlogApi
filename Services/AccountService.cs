using Azure;
using blog_api.Exeptions;
using Microsoft.EntityFrameworkCore;
using Response = blog_api.Models.Response.Response;

public class AccountService(DataContext context, IPasswordHasher passwordHasher, ITokenService tokenService) : IAccountService
{
    public async Task<TokenResponse> Register(UserRegisterModel userModel)
    {
        if (await context.Users.FirstOrDefaultAsync(user => user.Email == userModel.Email) is not null)
        {
            throw new ProfileAlreadyExistsExeption(ErrorConstants.ProfileAlreadyExistsError);
        }
        var hashedPassword = passwordHasher.Generate(userModel.Password);
        var user = UserMapper.MapFromRegisterModelToEntity(userModel, hashedPassword);
        Console.WriteLine(user);

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var token = new TokenResponse { Token = tokenService.GenerateToken(user) };

        return token;

    }

    public async Task<TokenResponse> Login(LoginCredentials loginCredentials)
    {
        if (await context.Users.FirstOrDefaultAsync(user => user.Email == loginCredentials.Email) is null)
        {
            throw new ProfileNotExistsExeption(ErrorConstants.ProfileNotExistsError);
        }
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email == loginCredentials.Email);
        var isPasswordTrue = passwordHasher.Verify(loginCredentials.Password, user.Password);
        
        if (!isPasswordTrue)
        {
            throw new PasswordNotExistsExeption(ErrorConstants.PasswordNotExistsError);
        }
        var token = new TokenResponse { Token = tokenService.GenerateToken(user) };
        return token;
    }
    
    public async Task<Response> Logout(string token)
    {
        //if (await tokenService.IsTokenBanned(token));
        context.BannedTokens.Add(new TokenEntity{Token = token});
        await context.SaveChangesAsync();
        return await Task.FromResult(new Response(null, "Logout successful"));
    }

    public async Task<UserDto> GetProfile(string token)
    {


        return new UserDto();
    }
}