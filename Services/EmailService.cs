
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using RCM.API.Interfaces;

namespace RCM.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmailAsync(
            string email,
            string resetLink)
        {
            string senderEmail =
                _configuration["EmailSettings:SenderEmail"]!;

            string appPassword =
                _configuration["EmailSettings:AppPassword"]!;

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(senderEmail);
                message.To.Add(email);

                message.Subject = "RCM System - Password Reset";

                message.Body = $@"
                    <h2>Password Reset Request</h2>
                    <p>We received a request to reset your password.</p>
                    <p>Click the link below to reset your password:</p>
                    <a href='{WebUtility.HtmlEncode(resetLink)}'>
                        Reset Password
                    </a>
                    <p>This link is valid for 15 minutes.</p>
                    <p>If you did not request this, please ignore this email.</p>
                ";

                message.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(
                    "smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(
                        senderEmail,
                        appPassword
                    );

                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(message);
                }
            }
        }
    }
}
