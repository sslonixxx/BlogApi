
using blog_api.EmailService;
using blog_api.Exeptions;
using blog_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;

public class DataQuartzJob : IJob
{
    private readonly IEmailService _emailSenderService;
    private readonly DataContext _context;
    private readonly IAccountService _accountService;

    public DataQuartzJob(IEmailService emailSenderService, DataContext context, IAccountService accountService)
    {
        _emailSenderService = emailSenderService;
        _context = context;
        _accountService = accountService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Starting job execution...");
        var communityId = Guid.Parse(context.JobDetail.JobDataMap.GetString("communityId"));

        var userIds = await _context.CommunityUser
            .Where(c => c.CommunityId == communityId)
            .Select(c => c.UserId)
            .ToListAsync();

        var community = await _context.Communities.FirstOrDefaultAsync(c => c.Id == communityId);

        if (community == null) throw new CustomException("Community not found", 404);

        foreach (var userId in userIds)
        {
            var user = await _accountService.GetUserById(userId.ToString());
            if (user == null) continue;

            var subject = "New Post in " + community.Name;
            var body = $"A new post has been published in the community '{community.Name}'.";

            await _emailSenderService.Send(user.Email, subject, body);
            Console.WriteLine($"Email sent to {user.Email}");
        }
    }
}
