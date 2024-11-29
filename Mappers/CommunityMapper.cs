using blog_api.Entities;

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

    public static CommunityUserDto CommunityUserToCommunityUserDto(CommunityUser communityUser)
    {
        var CommunityUserDto = new CommunityUserDto
        {
            UserId = communityUser.UserId,
            CommunityId = communityUser.CommunityId,
            Role = communityUser.Role,
        };
        return CommunityUserDto;
    }
}