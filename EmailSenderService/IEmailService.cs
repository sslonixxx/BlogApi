namespace blog_api.EmailSenderService;

public interface IEmailService
{
    public Task Send(string toEmail, string subject, string body);
    public Task NotifyCommunity(Guid? communityId);

}