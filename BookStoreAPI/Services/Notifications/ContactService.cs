using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Email;
using BookStoreData.Data;
using BookStoreData.Models.Notifications;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Notifications
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        Task CreateContactMessage(Contact contact);
        Task AnswerToContact(int contactId, string content);
    }

    public class ContactService(BookStoreContext context, IEmailService emailService) : IContactService
    {
        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            return await context.Contact.Where(x => x.IsActive).ToListAsync();
        }
        public async Task AnswerToContact(int contactId, string content)
        {
            var contact = await context.Contact.FirstAsync(x => x.Id == contactId && x.IsActive);

            if (contact == null)
            {
                throw new BadRequestException("Wystąpił błąd z pobieraniem kontaktu.");
            }

            emailService.SendEmail(contact.Email, "Odpowiedź na kontakt", content);
        }
        public async Task CreateContactMessage(Contact contact)
        {
            await context.Contact.AddAsync(contact);
            ConfirmationOfContact(contact.ClientName, contact.Email);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        private void ConfirmationOfContact(string userName, string userEmail)
        {
            var emailBody = $"Witaj {userName}, otrzymaliśmy twoją wiadomość :). Postaramy się odpowiedzieć na nią jak najszybciej.";
            emailService.SendEmail(userEmail, "Kontakt", emailBody);
        }
    }
}
