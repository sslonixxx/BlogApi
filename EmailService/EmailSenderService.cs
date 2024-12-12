
using System.Net.Mail;
using blog_api.BackgroundJob;
using blog_api.EmailService;
using blog_api.Exeptions;
using blog_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace blog_api.Services.Impls;

public class EmailSenderService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;
    private readonly string _username;
    private readonly SmtpClient smtpClient;
    public DataContext _context;
    private readonly IAccountService _accountService;

    public EmailSenderService(EmailConfiguration emailConfig, IAccountService accountService, DataContext context)
    {
        _emailConfig = emailConfig; 
        smtpClient = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port);
        _username = emailConfig.UserName;
        smtpClient.EnableSsl = false;
        smtpClient.Timeout = 2000;
        _context = context;
        _accountService = accountService;
    }

    
    public async Task NotifyCommunity(Guid? communityId)
    {
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

            await Send(user.Email, subject, body);
            Console.WriteLine($"Email sent to {user.Email}");
        }
    }
    public async Task Send(string toEmail, string subject, string body)
    {
        MailMessage message = new MailMessage(_emailConfig.UserName, toEmail, subject, body);
        smtpClient.Send(message);
    }

}
