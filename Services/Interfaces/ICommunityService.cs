namespace blog_api.Services.Interfaces;

public interface ICommunityService
{
    public Task<List<CommunityDto>> GetCommunities();
    public Task<CommunityDto?> GetCommunityInformation(Guid id);
    public Task<string?> GetUserRole(Guid id, string token);

    public Task SubscribeCommunity(Guid id, string token);
    public Task UnsubscribeCommunity(Guid id, string token);
    //public Task<List<CommunityUserDto>> GetMyCommunities(string token);


}