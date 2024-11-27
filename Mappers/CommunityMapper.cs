namespace blog_api.Mappers;

public class CommunityMapper
{
    public static CommunityDto CommunityToCommunityDto(Community community)
    {
        var CommunityDto = new CommunityDto()
        {
            Id = community.Id,
            Name = community.Name,
            Description = community.Description,
            IsClosed = community.IsClosed,
            SubscribersCount = community.SubscribersCount,
        };
        return CommunityDto;
    }
}