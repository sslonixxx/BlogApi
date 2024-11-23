using Azure;
using blog_api.Exeptions;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
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
        token = tokenService.ExtractTokenFromHeader(token);
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        context.BannedTokens.Add(new TokenEntity{Token = token});
        await context.SaveChangesAsync();
        return await Task.FromResult(new Response(null, "Logout successful"));
    }
    
    public async Task<User> GetUserByToken(string token)
    {
        if (await tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = tokenService.GetIdByToken(token);

        var user = await context.Users.FirstOrDefaultAsync(user => user.Id.ToString() == userId);
        if(user == null) throw new ProfileNotExistsExeption(ErrorConstants.ProfileNotExistsError);
        return user;
    }

    public async Task<UserDto> GetProfile(string token)
    {
        var user = await GetUserByToken(token);
        return UserMapper.MapFromEntityToDto(user);
    }

    public async Task<Response> EditProfile(UserEditModel userEditModel, string token)
    {
        var user = await GetUserByToken(token);
        user = UserMapper.MapFromEditModelToEntity(userEditModel, user);
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return await Task.FromResult(new Response(null, "Edit successful"));
    }
    
}