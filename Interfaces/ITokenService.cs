public interface ITokenService
{
    public string GenerateToken(User user);
    Task<bool> IsTokenBanned(TokenResponse token);
}