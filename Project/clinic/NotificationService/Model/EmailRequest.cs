namespace NotificationService.Model
{
    public class EmailRequest
    {
        public List<string> ToEmails { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SignatureHtml { get; set; } = null;
        public List<string> CcEmails { get; set; } = new List<string>();
        public List<string> BccEmails { get; set; } = new List<string>();
        public List<string> AttachmentPaths { get; set; } = new List<string>();
    }
}
