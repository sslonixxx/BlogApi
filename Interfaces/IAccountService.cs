public interface IAccountService
{
    public Task<TokenResponse> Register(UserRegisterModel user);
    public Task<TokenResponse> Login(LoginCredentials user);
}