using MimeKit;

namespace EmailService.Api.Services
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body, byte[] pdfBytes);
    }
}
