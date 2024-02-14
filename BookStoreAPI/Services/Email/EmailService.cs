using BookStoreDto.Dtos.Accounts.Account;
using System.Net.Mail;
using System.Net;

namespace BookStoreAPI.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
    public class EmailService(AccountEmailConfigurationDto emailConfiguration) : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            var smtpClient = ConfigureGmailSmtpClient();

            var message = CreateNewMessage(to, subject, body);

            smtpClient.Send(message);
        }

        private SmtpClient ConfigureGmailSmtpClient()
        {
            return new SmtpClient(emailConfiguration.SmtpServer)
            {
                Port = emailConfiguration.Port,
                Credentials = new NetworkCredential(emailConfiguration.Email, emailConfiguration.Password),
                EnableSsl = emailConfiguration.EnableSSL,
            };
        }

        private MailMessage CreateNewMessage(string to, string subject, string body)
        {
            return new MailMessage(emailConfiguration.Email, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
        }
    }
}
