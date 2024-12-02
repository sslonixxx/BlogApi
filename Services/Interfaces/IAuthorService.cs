namespace blog_api.Services.Interfaces;

public interface IAuthorService
{
    public Task<List<AuthorDto>> GetAuthors();
}