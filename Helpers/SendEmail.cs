using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace Helpers
{
    public static class SendEmail
    {
        public static async void SendMail(EmailAddress to, string subject, string plainTextContent, string htmlContent, string b64Content = null, string attachmentName = null)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var email = Environment.GetEnvironmentVariable("EMAIL");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(email, "Mortgage");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (!string.IsNullOrEmpty(attachmentName))
            {
                msg.AddAttachment(attachmentName, b64Content);
            }

            await client.SendEmailAsync(msg);
        }
    }
}
