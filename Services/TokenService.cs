using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

public class TokenService(IOptions<JwtOptions> options, DataContext context) : ITokenService

{
    private readonly JwtOptions _options = options.Value;
    public string GenerateToken(User user)
    {
        Claim[] claims = [new(ClaimTypes.Sid, user.Id.ToString())];
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
    
    public async Task<bool> IsTokenBanned(TokenResponse token)
    {
        return await context.BannedTokens.AnyAsync(t => t.Token == token.Token);
    }
    
}