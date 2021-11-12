using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMicroservice.Data
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(EmailConfiguration emailConfig,ILogger<EmailSender> logger)
        {
            _emailConfig = emailConfig;
            _logger = logger;
        }
        public void SendEmail(Message message)
        {
            _logger.LogInformation("EmailRepo--> Sending email ...");
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            _logger.LogInformation("EmailRepo--> Calling method for crating an Email message");
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            _logger.LogInformation("EmailRepo--> Starting method for sending a message");
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.Auto);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                    _logger.LogInformation("EmailRepo--> Mail message successfuly sent");
                }
                catch(Exception e)
                {
                    _logger.LogError($"EmailRepo--> An error occured ${e.Message}");
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                    _logger.LogInformation("EmailRepo--> Disposing resources");
                }
            }
        }
        public async Task SendEmailAsync(Message message)
        {
            _logger.LogInformation("EmailRepo--> Sending email async");
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }
        private async Task SendAsync(MimeMessage mailMessage)
        {
            _logger.LogInformation("EmailRepo--> Starting method for sending a message");
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.Auto);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch(Exception e)
                {
                    _logger.LogError($"EmailRepo--> An error occured ${e.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                    _logger.LogInformation("EmailRepo--> Disposing resources");
                }
            }
        }

        public async Task SendWelcomeEmailAsync(string to)
        {
            _logger.LogInformation("EmailRepo--> Method for sending a local Email");
            var message = new Message(new string[] { to},"Welcome email","We are happy that you have joined our shop.Happy shopping");
            await SendEmailAsync(message);
        }
    }
}
