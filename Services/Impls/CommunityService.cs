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

    public async Task<CommunityDto?> GetCommunityInformation(Guid id, string userId)
    {
        Community? community = context.Communities.FirstOrDefault(c => c.Id == id);
        var user = await accountService.GetUserById(userId);
        if (id != null && community.IsClosed &&
            (user == null || context.CommunityUser.Where(c => c.UserId == user.Id).All(uc => uc.CommunityId != community.Id))) {
            throw new CustomException("You don't have rights", 403);
        }
        if (community == null) throw new CustomException("Community not found", 400);
        return CommunityMapper.CommunityToCommunityDto(community);
    }

    public async Task<string?> GetUserRole(Guid id, string userId)
    {
        var user = await accountService.GetUserById(userId);
       var userCommunity =
           context.CommunityUser.Where(c => c.UserId == user.Id).FirstOrDefault(c => c.CommunityId == id);;
        return userCommunity?.Role.ToString() ?? null;
        
    }

    public async Task SubscribeCommunity(Guid id, string userId)
    {
        var user = await accountService.GetUserById(userId);
        var community = context.Communities.FirstOrDefault(c => c.Id == id);
        if (community == null) throw new CustomException("Community not found", 400);
        if (context.CommunityUser.Where(c=> c.CommunityId == community.Id).Any(s => s.UserId == user.Id)) throw new CustomException("User is already a subscriber", 400);
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
    
    public async Task UnsubscribeCommunity(Guid id, string userId)
    {
        var user = await accountService.GetUserById(userId);
        var community = context.Communities.Include(c => c.CommunityUser).FirstOrDefault(c => c.Id == id);
        if (community == null) throw new CustomException("Community not found", 400);
        var communityUser = context.CommunityUser.Where(c => c.CommunityId==community.Id).FirstOrDefault(c => c.UserId == user.Id);
        if (communityUser.Role == CommunityRole.Administrator) throw new CustomException($"User with this id not subscribe to the community {communityUser.CommunityId}", 400);
        if (communityUser == null) throw new CustomException("User not found", 400);
        
        community.CommunityUser.Remove(communityUser);
        community.SubscribersCount--;
        await context.SaveChangesAsync();
    }

    public async Task<List<CommunityUserDto>> GetMyCommunities(string userId)
    {
        var user = await accountService.GetUserById(userId);
        var communityUsers = context.CommunityUser.Where(c => c.UserId == user.Id).ToList();
        
        var userList = new List<CommunityUserDto>();
        foreach (var communityUser in communityUsers)
        {
            var communityUserDto = CommunityMapper.CommunityUserToCommunityUserDto(communityUser);
            userList.Add(communityUserDto);
        }
        return userList;
    }
}