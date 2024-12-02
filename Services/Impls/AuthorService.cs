using blog_api.Mappers;
using blog_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog_api.Services.Impls;

public class AuthorService(DataContext context): IAuthorService
{
    public async Task<List<AuthorDto>> GetAuthors()
    {
        var authors = await context.Users.ToListAsync();
        var authorList = new List<AuthorDto>();
        foreach (var author in authors)
        {
            var posts = await context.Posts
                .Where(p => p.AuthorId == author.Id)
                .ToListAsync();
            if (posts.Count>0) authorList.Add(AuthorMapper.Map(author, posts.Sum(p => p.Likes), posts.Count));
        }
        return authorList;
    }
}