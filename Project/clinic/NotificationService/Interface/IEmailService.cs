using NotificationService.Model;

namespace NotificationService.Interface
{
    public interface IEmailService
    {
        Task SendEmailNotification(EmailRequest emailRequest);
    }
}
