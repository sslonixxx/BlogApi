using Azure;
using Response = blog_api.Models.Response.Response;

public interface IAccountService
{
    public Task<TokenResponse> Register(UserRegisterModel user);
    public Task<TokenResponse> Login(LoginCredentials user);
    public Task<Response> Logout(string token);
    public Task<UserDto> GetProfile(string? token);
    public Task<Response> EditProfile(UserEditModel userEditModel, string token);
}