using EmailService.Api.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService.Api.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSetting _settings;

        public EmailSender(IOptions<EmailSetting> settings)
        {
             _settings = settings.Value;
        }

        public async Task SendAsync(string to, string subject, string body, byte[] pdfBytes)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            builder.Attachments.Add("Ticket.pdf", pdfBytes, ContentType.Parse("application/pdf"));
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _settings.SmtpServer,
                _settings.Port, 
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
               _settings.Username,
                _settings.Password
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
