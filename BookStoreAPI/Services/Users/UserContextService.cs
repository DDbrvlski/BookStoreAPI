using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreData.Models.Customers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace BookStoreAPI.Services.Users
{
    public interface IUserContextService
    {
        Task<Customer?> GetCustomerByTokenAsync();
        Task<User?> GetUserAndCustomerDataByTokenAsync();
        Task<User?> GetUserByDataAsync(Expression<Func<User, bool>> userExpression);
        Task<User?> GetUserByTokenAsync();
        Task<string> GetUserIdByTokenAsync();
    }

    public class UserContextService(BookStoreContext context, IHttpContextAccessor httpContextAccessor) : IUserContextService
    {
        public async Task<User?> GetUserByDataAsync(Expression<Func<User, bool>> userExpression)
        {
            return await context.User.Where(x => x.IsActive).FirstOrDefaultAsync(userExpression);
        }

        public async Task<User?> GetUserByTokenAsync()
        {
            var userId = await GetUserIdByTokenAsync();
            return await GetUserByDataAsync(x => x.Id == userId);
        }
        public async Task<string> GetUserIdByTokenAsync()
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userId == null)
            //{
            //    throw new BadRequestException("Wystąpił błąd z autoryzacją użytkownika.");
            //}

            return userId;
        }
        public async Task<User?> GetUserAndCustomerDataByTokenAsync()
        {
            var userId = await GetUserIdByTokenAsync();

            var user = await context.User.Include(x => x.Customer).FirstOrDefaultAsync(x => x.IsActive && x.Id == userId);

            if (user == null)
            {
                throw new BadRequestException("Nie znaleziono danych użytkownika.");
            }

            return user;
        }
        public async Task<Customer?> GetCustomerByTokenAsync()
        {
            var user = await GetUserByTokenAsync();
            if (user == null)
            {
                throw new AccountException("Nie znaleziono użytkownika.");
            }

            var customer = await context.Customer.FirstOrDefaultAsync(x => x.Id == user.CustomerID && x.IsActive);
            if (customer == null)
            {
                throw new AccountException("Nie znaleziono danych użytkownika.");
            }

            return customer;
        }
    }
}
