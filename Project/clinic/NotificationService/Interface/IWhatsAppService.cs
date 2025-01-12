namespace NotificationService.Interface
{
    public interface IWhatsAppService
    {
        void SendWhatsAppNotification(string whatsappNumber, string message);
    }
}
