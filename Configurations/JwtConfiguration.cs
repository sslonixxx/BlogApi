// using Microsoft.Extensions.Options;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
//
// public class JwtConfiguration
// {
//     public static void AddApiAuthentication(
//         IServiceCollection services,
//         IOptions<JwtOptions> jwtOptions)
//     {
//         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//             .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//             {
//                 options.TokenValidationParameters = new()
//                 {
//                     ValidateIssuer = true,
//                     ValidateAudience = true,
//                     ValidateLifetime = true,
//                     ValidateIssuerSigningKey = true,
//                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
//
//                 };
//             });
//
//     }
//
// }