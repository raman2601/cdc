namespace NotificationService.Interface
{
    public interface ISmsService
    {
        void SendSmsNotification(string phoneNumber, string message);
    }
}
