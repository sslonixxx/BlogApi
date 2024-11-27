using blog_api.Exeptions;
using blog_api.Mappers;
using blog_api.Services.Interfaces;

namespace blog_api.Services.Impls;

public class CommunityService(DataContext context) : ICommunityService
{
    public async Task<List<CommunityDto>> GetCommunities()
    {
        var communities = context.Communities.Select(t => CommunityMapper.CommunityToCommunityDto(t)).ToList();
        return communities;
    }

    public async Task<CommunityDto?> GetCommunityInformation(Guid id)
    {
        var community = context.Communities.FirstOrDefault(t => t.Id == id);
        if (community == null) throw new CustomException("Community not found", 400);
        return CommunityMapper.CommunityToCommunityDto(community);
    }
}