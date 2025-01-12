namespace NotificationService.Model
{
    public class NotificationRequest
    {
        public EmailRequest EmailRequest { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string WhatsAppNumber { get; set; }
        public string SignalRHub { get; set; }
    }

}
