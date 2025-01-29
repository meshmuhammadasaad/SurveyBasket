using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SurveyBasket.Api.Setting;

namespace SurveyBasket.Api.Services;

public class EmailService(IOptions<MailSettings> mailSettings) : IEmailSender
{
    private readonly MailSettings _emailSettings = mailSettings.Value;

    public async Task SendEmailAsync(string Email, string subject, string htmlMessage)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Host));
        email.To.Add(MailboxAddress.Parse(Email));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.Mail, _emailSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.Host, _emailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
