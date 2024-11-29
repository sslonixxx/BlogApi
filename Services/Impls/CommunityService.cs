using blog_api.Entities;
using blog_api.Exeptions;
using blog_api.Mappers;
using blog_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace blog_api.Services.Impls;

public class CommunityService(DataContext context, IAccountService accountService, ITokenService tokenService) : ICommunityService
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

    public async Task<string?> GetUserRole(Guid id, string token)
    {
        var user = await accountService.GetUserByToken(token);
        var userCommunity = user.CommunityUser.FirstOrDefault(c => c.CommunityId == id);
        return userCommunity?.Role.ToString() ?? null;
        
    }

    public async Task SubscribeCommunity(Guid id, string token)
    {
        var user = await accountService.GetUserByToken(token);
        var community = context.Communities.FirstOrDefault(c => c.Id == id);
        if (community == null) throw new CustomException("Community not found", 400);
        if (community.CommunityUser.Any(s => s.UserId == user.Id)) throw new CustomException("User is already a subscriber", 400);
        CommunityUser communityUser = new CommunityUser()
        {
            UserId = user.Id,
            CommunityId = community.Id,
            Role = CommunityRole.Subscriber,
            User = user,
            Community = community
        };
        community.CommunityUser.Add(communityUser);
        community.SubscribersCount++;
        await context.SaveChangesAsync();
    }
    
    public async Task UnsubscribeCommunity(Guid id, string token)
    {
        var user = await accountService.GetUserByToken(token);
        var community = context.Communities.Include(c => c.CommunityUser).FirstOrDefault(c => c.Id == id);
        if (community == null) throw new CustomException("Community not found", 400);
        var communityUser = community.CommunityUser.FirstOrDefault(c => c.UserId == user.Id);
        if (communityUser == null) throw new CustomException("User not found", 400);
        var admins = community.CommunityUser.Where(uc => uc.Role == CommunityRole.Administrator).ToList();
        if (communityUser.Role == CommunityRole.Subscriber && admins.Count == 1)
        {
            var randomUser = community.CommunityUser.FirstOrDefault(c => c.Role == CommunityRole.Subscriber);
            randomUser.Role = CommunityRole.Administrator;
        }
        community.CommunityUser.Remove(communityUser);
        community.SubscribersCount--;
        await context.SaveChangesAsync();
    }
    
    public Task<List<Community>> GetUserCommunityList(Guid userId)
    {
        return Task.FromResult(context.Communities
            .Include(c => c.CommunityUser)!
            .ThenInclude(m => m.User)
            .Where(c => c.CommunityUser!.Any(m => m.UserId == userId))
            .ToList());
    }

    // public async Task<List<CommunityUserDto>> GetMyCommunities(string token)
    // {
    //     var user = await accountService.GetUserByToken(token);
    //     var communities = await GetUserCommunityList(user.Id);
    //
    //     var userList = new List<CommunityUserDto>();
    //     foreach (var community in communities)
    //     {
    //         var communityUser = community.CommunityUser.
    //         userList.Add(communityUser);
    //     }
    //     return userList;
    // }
}