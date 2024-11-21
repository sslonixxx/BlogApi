using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.AddHttpContextAccessor();
services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


JwtOptions jwtOptions = new();
configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);

//services.AddSingleton(jwtOptions);
//configuration.Get<JwtOptions>(configuration.GetSection(""));
//JwtConfigurarion.AddApiAuthentication(services, services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog.API", Version = "v1" });
    c.EnableAnnotations();
});

services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IAccountService, AccountService>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.MapControllers();

app.Run();
