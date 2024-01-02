using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Customers;
using BookStoreAPI.Services.Email;
using BookStoreAPI.Services.Users;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreData.Models.Notifications;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Notifications
{
    public interface INewsletterService
    {
        Task AddNewsletterUserAsync(string email);
        Task CreateNewsletterAsync(Newsletter newsletter);
        Task AddToNewsletterSubscribersAsync(string email);
        Task RemoveFromNewsletterSubscribersAsync(string email);
        Task SendNewsletterToSubscribersAsync();
        Task<bool> IsAlreadySubscribed(string email);
        Task<IEnumerable<Newsletter>> GetAllNewslettersAsync();
        Task EditNewsletterAsync(int newsletterId, Newsletter newNewsletter);
    }
    public class NewsletterService
        (BookStoreContext context,
        IEmailService emailSender)
        : INewsletterService
    {
        public async Task<IEnumerable<Newsletter>> GetAllNewslettersAsync()
        {
            return await context.Newsletter.Where(x => x.IsActive).ToListAsync();
        }
        public async Task EditNewsletterAsync(int newsletterId, Newsletter newNewsletter)
        {
            var newsletter = await context.Newsletter.Where(x => x.IsActive && x.Id == newsletterId).FirstAsync();
            newsletter.CopyProperties(newNewsletter);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task AddNewsletterUserAsync(string email)
        {
            var customer = await context.Customer.FirstOrDefaultAsync(x => x.Email == email && x.IsActive);

            if (customer != null)
            {
                await SetCustomerNewsletterSubscriptionAsync(customer);
            }

            if (await IsAlreadySubscribed(email))
            {
                throw new BadRequestException("E-mail jest już zasubskrybowany do newslettera.");
            }

            await AddToNewsletterSubscribersAsync(email);
        }

        public async Task CreateNewsletterAsync(Newsletter newsletter)
        {
            await context.Newsletter.AddAsync(newsletter);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task AddToNewsletterSubscribersAsync(string email)
        {
            var newNewsletterUser = new NewsletterSubscribers()
            {
                Email = email,
            };

            await context.NewsletterSubscribers.AddAsync(newNewsletterUser);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task RemoveFromNewsletterSubscribersAsync(string email)
        {
            var subscriberToRemove = await context.NewsletterSubscribers.FirstOrDefaultAsync(x => x.Email == email && x.IsActive);
            if (subscriberToRemove == null)
            {
                throw new BadRequestException("Podany adres e-mail nie jest zasubskrybowany do newslettera.");
            }

            subscriberToRemove.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task SendNewsletterToSubscribersAsync()
        {
            //var newslettersToSend = await context.Newsletter.Where(x => x.IsActive && x.PublicationDate.ToShortDateString == DateTime.Now.ToShortDateString).ToListAsync();

            //Wysyłanie ostatniego newslettera w ramach testu
            var newslettersToSend = await context.Newsletter.OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.IsActive);
            var newsletterSubscribers = await context.NewsletterSubscribers.Where(x => x.IsActive).ToListAsync();

            if (newslettersToSend != null && newsletterSubscribers != null)
            {
                foreach (var subscriber in newsletterSubscribers)
                {
                    emailSender.SendEmail(subscriber.Email, newslettersToSend.Title, newslettersToSend.Content);
                }
            }
        }

        public async Task<bool> IsAlreadySubscribed(string email) =>
            (await context.NewsletterSubscribers.FirstOrDefaultAsync(x => x.Email == email && x.IsActive)) != null;

        private async Task SetCustomerNewsletterSubscriptionAsync(Customer customer)
        {
            if (customer.IsSubscribed)
            {
                throw new BadRequestException("E-mail jest już zasubskrybowany do newslettera.");
            }
            else
            {
                customer.IsSubscribed = true;
                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
    }
}
