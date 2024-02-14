using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Addresses;
using BookStoreAPI.Services.Notifications;
using BookStoreAPI.Services.Users;
using BookStoreAPI.Services.Wishlists;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreDto.Dtos.Customers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStoreAPI.Services.Customers
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync(CustomerPostDto customerPost);
        Task DeactivateCustomerAsync(int customerId);
        Task<Customer?> GetCustomerByDataAsync(Expression<Func<Customer, bool>> customerFunction);
        Task<Customer> GetCustomerByTokenAsync();
        Task<int> CreateCustomerHistoryAsync(int customerId);
    }

    public class CustomerService
        (BookStoreContext context,
        IWishlistService wishlistService,
        IUserContextService userContextService,
        INewsletterService newsletterService,
        IAddressService addressService)
        : ICustomerService
    {
        public async Task<Customer> CreateCustomerAsync(CustomerPostDto customerPost)
        {
            Customer customer = new();
            customer.CopyProperties(customerPost);

            context.Customer.Add(customer);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            CustomerHistory customerHistory = new();
            customerHistory.CopyProperties(customerPost);
            customerHistory.CustomerID = customer.Id;

            await context.CustomerHistory.AddAsync(customerHistory);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            if (customer.IsSubscribed)
            {
                if (!await newsletterService.IsAlreadySubscribed(customer.Email))
                {
                    await newsletterService.AddToNewsletterSubscribersAsync(customer.Email);
                }
            }

            await wishlistService.CreateWishlistAsync(customer);

            return customer;
        }
        public async Task DeactivateCustomerAsync(int customerId)
        {
            var customer = await GetCustomerByDataAsync(x => x.Id == customerId);

            if (customer == null)
            {
                throw new BadRequestException("Nie znaleziono danych użytkownika.");
            }

            await wishlistService.DeactivateWishlistAsync(customerId);
            await newsletterService.RemoveFromNewsletterSubscribersAsync(customer.Email);
            await addressService.DeactivateAllAddressesForCustomerAsync(customerId);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task<Customer> GetCustomerByTokenAsync()
        {
            var user = await userContextService.GetUserByTokenAsync();
            if (user == null)
            {
                throw new AccountException("Nie znaleziono użytkownika.");
            }

            var customer = await GetCustomerByDataAsync(x => x.Id == user.CustomerID);
            if (customer == null)
            {
                throw new AccountException("Nie znaleziono danych użytkownika.");
            }

            return customer;
        }
        public async Task<Customer?> GetCustomerByDataAsync(Expression<Func<Customer, bool>> customerFunction)
        {
            return await context.Customer.Where(x => x.IsActive).FirstOrDefaultAsync(customerFunction);
        }
        public async Task<int> CreateCustomerHistoryAsync(int customerId)
        {
            var customer = await GetCustomerByDataAsync(x => x.Id == customerId);
            if (customer == null)
            {
                throw new AccountException("Nie znaleziono danych użytkownika.");
            }

            var oldHistory = await context.CustomerHistory.FirstOrDefaultAsync(x => x.IsActive && x.CustomerID == customerId);
            if (oldHistory != null)
            {
                oldHistory.IsActive = false;
            }

            CustomerHistory customerHistory = new();
            customerHistory.CopyProperties(customer);
            customerHistory.CustomerID = customer.Id;

            context.CustomerHistory.Add(customerHistory);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            return customerHistory.Id;
        }

    }
}
