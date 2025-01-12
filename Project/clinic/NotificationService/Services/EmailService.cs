using MimeKit;
using NotificationService.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using NotificationService.Model;

namespace NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly string SmtpServer;
        private readonly int SmtpPort;
        private readonly string Username;
        private readonly string Password;
        private readonly string Name;
        public EmailService(string username,string password,string server,int port,string name)
        {
            Username = username;
            Password = password;
            SmtpPort = port;
            SmtpServer=server;
            Name = name;
        }
        public async Task SendEmailNotification(EmailRequest emailRequest)
        {
            var email = new MimeMessage();

            // Add sender
            email.From.Add(new MailboxAddress(Name,Username));

            // Add To recipients
            foreach (var toEmail in emailRequest.ToEmails)
            {
                email.To.Add(new MailboxAddress("", toEmail));
            }

            // Add CC recipients (if provided)
            if (emailRequest.CcEmails != null)
            {
                foreach (var ccEmail in emailRequest.CcEmails)
                {
                    email.Cc.Add(new MailboxAddress("", ccEmail));
                }
            }

            // Add BCC recipients (if provided)
            if (emailRequest.BccEmails != null)
            {
                foreach (var bccEmail in emailRequest.BccEmails)
                {
                    email.Bcc.Add(new MailboxAddress("", bccEmail));
                }
            }

            // Set subject
            email.Subject = emailRequest.Subject;

            // Build email body
            var builder = new BodyBuilder
            {
                HtmlBody = $"{emailRequest.Body}<br><br>{emailRequest.SignatureHtml ?? ""}" // Append signature if provided
            };

            // Attach files (if provided)
            if (emailRequest.AttachmentPaths != null)
            {
                foreach (var attachmentPath in emailRequest.AttachmentPaths)
                {
                    if (File.Exists(attachmentPath))
                    {
                        builder.Attachments.Add(attachmentPath);
                    }
                    else
                    {
                        throw new FileNotFoundException($"Attachment not found: {attachmentPath}");
                    }
                }
            }

            email.Body = builder.ToMessageBody();

            // Send email using SMTP
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(SmtpServer, SmtpPort, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(Username, Password);
                await smtpClient.SendAsync(email);
                await smtpClient.DisconnectAsync(true);
            }
        }

    }
    
}
