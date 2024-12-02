namespace blog_api.Mappers;

public class AuthorMapper
{
    public static AuthorDto Map(User author, int likes, int size)
    {
        return new AuthorDto()
        {
            Name = author.Name,
            BirthDate = author.BirthDate,
            Gender = author.Gender,
            Posts = size,
            Likes = likes,
            Created = author.CreateTime
        };
    }
}