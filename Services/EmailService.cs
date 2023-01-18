using Connectify.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Connectify.Services
{
    public class EmailService : IEmailSender
    { 
        private readonly MailSettings? _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailSender = _mailSettings.EMail;

            MimeMessage newEmail = new();

            newEmail.Sender = MailboxAddress.Parse(emailSender);

            foreach (var emailAdress in email.Split(";"))
            {
                newEmail.To.Add(MailboxAddress.Parse(emailAdress));
            }

            newEmail.Subject = subject;

            BodyBuilder emailBody = new();
            emailBody.HtmlBody = htmlMessage;

            newEmail.Body = emailBody.ToMessageBody();

            //now we log into our smtp client
            using SmtpClient smtpClient = new();

            try
            {
                var host = _mailSettings.Host;
                var port = _mailSettings.Port;
                var password = _mailSettings.Password;

                await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(emailSender, password);

                await smtpClient.SendAsync(newEmail);
                await smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                throw;
            }
        }
    }
}
