using Microsoft.EntityFrameworkCore;

public class AccountService(DataContext context, IPasswordHasher passwordHasher, ITokenService tokenService) : IAccountService
{
    public async Task<TokenResponse> Register(UserRegisterModel userModel)
    {
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
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email == loginCredentials.Email);
        var result = passwordHasher.Verify(loginCredentials.Password, user.Password);

        var token = new TokenResponse { Token = tokenService.GenerateToken(user) };
        return token;
    }
}