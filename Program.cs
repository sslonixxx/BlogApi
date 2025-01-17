using System.IdentityModel.Tokens.Jwt;
using System.Text;
using blog_api.EmailSenderService;
using blog_api.Middleware;
using blog_api.Models.Fias;
using blog_api.Services.Impls;
using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();


services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.AddHttpContextAccessor();
services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
services.AddDbContext<AddressContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("FiasConnection")));



JwtOptions jwtOptions = new();
configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
services.AddSingleton<JwtSecurityTokenHandler>();
services.AddSingleton(provider =>
{
    return new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey)),
        ValidIssuer = jwtOptions.Issuer,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidAudience = jwtOptions.Audience,
        ValidateAudience = true
    };
});

services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey)),
        ValidIssuer = jwtOptions.Issuer,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidAudience = jwtOptions.Audience,
        ValidateAudience = true
    };
});

services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IAccountService, AccountService>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<ITagService, TagService>();
services.AddScoped<ICommunityService, CommunityService>();
services.AddScoped<IPostService, PostService>();
services.AddScoped<ICommentService, CommentService>();
services.AddScoped<IAuthorService, AuthorService>();
services.AddScoped<IAddressService, AddressService>();
services.AddEndpointsApiExplorer();
services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
services.AddSingleton(provider => provider.GetRequiredService<ISchedulerFactory>().GetScheduler().Result);
services.AddScoped<IEmailService, EmailSenderService>();


services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var emailConfig = configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
services.AddSingleton(emailConfig);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExeptionMiddleware>();

app.Run();


