using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Addresses;
using BookStoreAPI.Services.Auth;
using BookStoreAPI.Services.Customers;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Accounts.User;
using BookStoreDto.Dtos.Customers;
using BookStoreDto.Dtos.Customers.Address;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;

namespace BookStoreAPI.Services.Users
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserPostDto userPost);
        Task DeactivateUserAccountAsync();
        Task EditUserAddressDataAsync(UserAddressDto userData);
        Task EditUserDataAsync(UserDataDto userData);
        Task EditUserPasswordAsync(UserChangePasswordDto userData);
        Task<IEnumerable<AddressDetailsDto>> GetUserAddressDataAsync();
        Task<UserDataDto> GetUserDataAsync();
        Task ValidateUserFieldsAsync(string username, string email, string userId = "");
    }

    public class UserService
            (BookStoreContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ICustomerService customerService,
            IUserContextService userContextService,
            IAddressService addressService)
            : IUserService
    {
        public async Task<UserDataDto> GetUserDataAsync()
        {
            var user = await userContextService.GetUserAndCustomerDataByTokenAsync();

            return new UserDataDto()
            {
                Name = user.Customer.Name,
                Surname = user.Customer.Surname,
                Email = user.Customer.Email,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,
            };
        }
        public async Task<IEnumerable<AddressDetailsDto>> GetUserAddressDataAsync()
        {
            var user = await userContextService.GetUserByTokenAsync();
            if (user == null)
            {
                throw new AccountException("Nie znaleziono użytkownika.");
            }

            return await context.CustomerAddress
                .Where(x => x.IsActive && x.CustomerID == user.CustomerID && (x.Address.AddressTypeID == 1 || x.Address.AddressTypeID == 2))
                .OrderBy(x => x.Address.AddressTypeID)
                .Select(x => new AddressDetailsDto()
                {
                    Id = (int)x.AddressID,
                    AddressTypeID = x.Address.AddressTypeID,
                    CityID = x.Address.CityID,
                    CityName = x.Address.City.Name,
                    CountryID = x.Address.CountryID,
                    CountryName = x.Address.Country.Name,
                    HouseNumber = x.Address.HouseNumber,
                    Postcode = x.Address.Postcode,
                    Street = x.Address.Street,
                    StreetNumber = x.Address.StreetNumber,
                })
                .ToListAsync();

        }
        public async Task DeactivateUserAccountAsync()
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var user = await userContextService.GetUserByTokenAsync();
                    if (user == null)
                    {
                        throw new AccountException("Nie znaleziono użytkownika.");
                    }

                    user.IsActive = false;

                    await customerService.DeactivateCustomerAsync((int)user.CustomerID);

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }            
        }
        public async Task<User> CreateUserAsync(UserPostDto userPost)
        {
            User user = new()
            {
                Email = userPost.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userPost.Username,
                CustomerID = userPost.CustomerId,
                PhoneNumber = userPost.PhoneNumber,
            };

            var createUserResult = await userManager.CreateAsync(user, userPost.Password);

            if (!createUserResult.Succeeded)
            {
                throw new AccountException("Wystąpił błąd podczas tworzenia konta użytkownika.");
            }
            else
            {
                var role = await roleManager.FindByNameAsync(userPost.RoleName);
                if (role == null)
                {
                    throw new AccountException("Nie znaleziono podanej roli.");
                }

                await userManager.AddToRoleAsync(user, userPost.RoleName);

                return user;
            }
        }
        public async Task EditUserDataAsync(UserDataDto userData)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var user = await userContextService.GetUserAndCustomerDataByTokenAsync();
                    string userId = user.Id;

                    if (userData.UserId != null || user.Id != userData.UserId)
                    {
                        userId = userData.UserId;
                        user = await userContextService.GetUserByDataAsync(x => x.Id == userData.UserId);
                    }

                    await ValidateUserFieldsAsync(userData.Username, userData.Email, userId);
                    await customerService.CreateCustomerHistoryAsync(user.CustomerID);

                    user.UserName = userData.Username;
                    user.Email = userData.Email;
                    user.PhoneNumber = userData.PhoneNumber;
                    user.Customer.Name = userData.Name;
                    user.Customer.Surname = userData.Surname;
                    user.Customer.PhoneNumber = userData.PhoneNumber;
                    user.Customer.Email = userData.Email;

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }            
        }
        public async Task EditUserPasswordAsync(UserChangePasswordDto userData)
        {
            var user = await userContextService.GetUserByTokenAsync();
            if (user == null)
            {
                throw new AccountException("Nie znaleziono użytkownika.");
            }

            var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, userData.OldPassword);
            if (!isCurrentPasswordValid)
            {
                throw new AccountException("Aktualne hasło jest nieprawidłowe.");
            }

            if (userData.NewPassword != userData.RepeatNewPassword)
            {
                throw new AccountException("Nowe hasło i powtórzone nowe hasło nie są identyczne.");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, userData.OldPassword, userData.NewPassword);
            user.SecurityStamp = Guid.NewGuid().ToString();

            if (!changePasswordResult.Succeeded)
            {
                throw new AccountException("Nie udało się zmienić hasła użytkownika");
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task EditUserAddressDataAsync(UserAddressDto userData)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var user = await userContextService.GetUserByTokenAsync();
                    if (user == null)
                    {
                        throw new AccountException("Nie znaleziono użytkownika.");
                    }

                    var customerAddresses = await addressService.GetCustomerAddressDataAsync((int)user.CustomerID);

                    if (userData.address == null)
                    {
                        throw new AccountException("Wystąpił błąd z pobieraniem adresu");
                    }

                    if (userData.mailingAddress == null)
                    {
                        userData.mailingAddress = new();
                        userData.mailingAddress.CopyProperties(userData.address);
                    }

                    userData.mailingAddress.AddressTypeID = 2;
                    List<BaseAddressDto> addresses = [userData.address, userData.mailingAddress];
                    await customerService.CreateCustomerHistoryAsync((int)user.CustomerID);

                    if (!customerAddresses.IsNullOrEmpty())
                    {
                        await addressService.UpdateAddressesForCustomerAsync((int)user.CustomerID, addresses);
                    }
                    else
                    {
                        await addressService.AddAddressesForCustomerAsync((int)user.CustomerID, addresses);
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }            
        }

        public async Task ValidateUserFieldsAsync(string username, string email, string userId = "")
        {
            List<string> errorMessages = new();

            var isUsernameValid = await userContextService.GetUserByDataAsync(x => x.UserName == username && x.Id != userId);
            if (isUsernameValid != null)
            {
                errorMessages.Add("Podana nazwa użytkownika jest już zajęta.");
            }

            var isEmailValid = await userContextService.GetUserByDataAsync(x => x.Email == email && x.Id != userId);
            if (isEmailValid != null)
            {
                errorMessages.Add("Podany email jest już zajęty.");
            }

            if (errorMessages.Any())
            {
                throw new AccountException(errorMessages);
            }
        }
    }
}
