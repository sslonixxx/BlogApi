using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using blog_api.Exeptions;
using Microsoft.EntityFrameworkCore;

public class TokenService(IOptions<JwtOptions> options, DataContext context, JwtSecurityTokenHandler tokenHandler, TokenValidationParameters tokenValidationParameters) : ITokenService

{
    private readonly JwtOptions _options = options.Value;
    public string GenerateToken(User user)
    {
        // Используем ClaimTypes.Sid, как вы хотите
        Claim[] claims = new Claim[]
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString())  // Здесь используем ClaimTypes.Sid
        };

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

    
    public async Task<bool> IsTokenBanned(string token)
    {
        return await context.BannedTokens.AnyAsync(t => t.Token == token);
    }
    
    public string GetIdByToken(string token)
    {
        if (!tokenHandler.CanReadToken(token)) throw new CustomException("Invalid token", 401);
        
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
        if (userIdClaim == null)
        {
            // Выведите информацию о всех claims в токене для дебага
            Console.WriteLine("SID claim not found. Claims in the token:");
            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }
            throw new CustomException("Invalid token: SID claim missing", 401);
        }
        var userId = userIdClaim.Value;        if(userId == null) throw new CustomException("Invalid token", 401);
            
        return userId;
    }
    
    public string ExtractTokenFromHeader(string authorizationHeader)
    {
        if (string.IsNullOrEmpty(authorizationHeader))
            throw new CustomException("Authorization header is missing", 401);

        if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            throw new CustomException("Invalid authorization header format", 401);

        return authorizationHeader.Substring("Bearer ".Length).Trim();
    }
    public bool ValidateToken(string? token)
    {
        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}