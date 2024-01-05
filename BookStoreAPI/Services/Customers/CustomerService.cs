using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Addresses;
using BookStoreAPI.Services.Notifications;
using BookStoreAPI.Services.Users;
using BookStoreAPI.Services.Wishlists;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreData.Models.Customers;
using BookStoreViewModels.ViewModels.Customers;
using BookStoreViewModels.ViewModels.Customers.Address;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStoreAPI.Services.Customers
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync(CustomerPostViewModel customerPost);
        Task DeactivateCustomerAsync(int customerId);
        Task<Customer?> GetCustomerByDataAsync(Expression<Func<Customer, bool>> customerFunction);
        Task<Customer> GetCustomerByTokenAsync();
    }

    public class CustomerService
        (BookStoreContext context,
        IWishlistService wishlistService,
        IUserContextService userContextService,
        INewsletterService newsletterService,
        IAddressService addressService)
        : ICustomerService
    {
        public async Task<Customer> CreateCustomerAsync(CustomerPostViewModel customerPost)
        {
            Customer customer = new();
            customer.CopyProperties(customerPost);

            context.Customer.Add(customer);
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

    }
}
