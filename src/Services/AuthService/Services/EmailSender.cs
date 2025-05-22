using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace AuthService.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSenderOptions _options;

    public EmailSender(IOptions<EmailSenderOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_options.SmtpUser, _options.SmtpPass)
        };

        await client.SendMailAsync(
            new MailMessage(_options.FromEmail, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            }
        );
    }
}

public class EmailSenderOptions
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587; // default port for SMTP
    public string SmtpUser { get; set; } = string.Empty;
    public string SmtpPass { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
}
