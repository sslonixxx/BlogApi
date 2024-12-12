using blog_api.EmailService;

namespace blog_api.Services.Interfaces;

public interface IEmailService
{
    public Task Send(string toEmail, string subject, string body);
    public Task NotifyCommunity(Guid? communityId);


}