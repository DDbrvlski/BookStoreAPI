using BookStoreData.Models.Accounts;
using System.Web;

namespace BookStoreAPI.Services.Email
{
    public interface IEmailSenderService
    {
        Task ResetPasswordEmail(string token, User user);
        Task ConfirmEmailEmail(string token, User user);
    }
    public class EmailSenderService
        (IEmailService emailService)
        : IEmailSenderService
    {
        public async Task ResetPasswordEmail(string token, User user)
        {
            if (emailService.IsEmailConfigurationSet())
            {
                var resetLink = $"http://localhost:3000/dostep/odzyskaj-konto/resetuj-haslo?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
                var emailBody = $"Aby zresetować hasło, kliknij <a href='{resetLink}'>tutaj</a>.";
                emailService.SendEmail(user.Email, "Zresetuj hasło", emailBody);
            }
        }
        public async Task ConfirmEmailEmail(string token, User user)
        {
            if (emailService.IsEmailConfigurationSet())
            {
                var confirmationLink = $"http://localhost:3000/dostep/rejestracja/potwierdzenie?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
                var emailBody = $"Aby potwierdzić adres email, klinij <a href='{confirmationLink}'>tutaj</a>.";
                emailService.SendEmail(user.Email, "Potwierdź email", emailBody);
            }
        }
    }
}
