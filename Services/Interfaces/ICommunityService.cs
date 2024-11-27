namespace blog_api.Services.Interfaces;

public interface ICommunityService
{
    public Task<List<CommunityDto>> GetCommunities();
    public Task<CommunityDto?> GetCommunityInformation(Guid id);
}