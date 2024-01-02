using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreViewModels.ViewModels.Customers.Address;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Addresses
{
    public interface IAddressService
    {
        Task AddAddressesForCustomerAsync(int customerId, List<BaseAddressViewModel> addresses);
        Task UpdateAddressesForCustomerAsync(int customerId, List<BaseAddressViewModel> newAddresses);
        Task DeactivateAllAddressesForCustomerAsync(int customerId);
        Task DeactivateChosenAddressForCustomerAsync(int customerId, int addressId);
        Task<IEnumerable<BaseAddressViewModel>> GetCustomerAddressDataAsync(int customerId);
    }
    public class AddressService(BookStoreContext context) : IAddressService
    {
        public async Task<IEnumerable<BaseAddressViewModel>> GetCustomerAddressDataAsync(int customerId)
        {
            return await context.CustomerAddress
                .Where(x => x.CustomerID == customerId && x.IsActive)
                .OrderBy(x => x.Address.Position)
                .Select(x => new BaseAddressViewModel()
                {
                    Id = (int)x.AddressID,
                    Position = x.Address.Position,
                    CityID = x.Address.CityID,
                    CountryID = x.Address.CountryID,
                    HouseNumber = x.Address.HouseNumber,
                    Postcode = x.Address.Postcode,
                    Street = x.Address.Street,
                    StreetNumber = x.Address.StreetNumber,
                })
                .ToListAsync();
        }
        public async Task AddAddressesForCustomerAsync(int customerId, List<BaseAddressViewModel> addresses)
        {
            if (addresses.Any())
            {
                var newAddresses = addresses
                    .Where(address => address != null)
                    .Select(address => new Address
                    {
                    }
                    .CopyProperties(address))
                    .ToList();

                context.Address.AddRange(newAddresses);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                var customerAddresses = newAddresses
                    .Select(address => new CustomerAddress
                    {
                        CustomerID = customerId,
                        AddressID = address.Id
                    })
                    .ToList();

                context.CustomerAddress.AddRange(customerAddresses);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        public async Task UpdateAddressesForCustomerAsync(int customerId, List<BaseAddressViewModel> newAddresses)
        {
            if (newAddresses.Any())
            {
                var oldAddresses = await context.CustomerAddress
                    .Where(x => x.IsActive && x.CustomerID == customerId)
                    .ToListAsync();

                var address = oldAddresses.First(x => x.Address.Position == 1);
                var maillingAddress = oldAddresses.First(x => x.Address.Position == 2);

                var newAddress = newAddresses.First(x => x.Position == 1);
                var newMaillingAddress = newAddresses.First(x => x.Position == 2);

                List<BaseAddressViewModel> addressesToAdd = new();

                if (!AreAddressesEqual(address.Address, newAddress))
                {
                    await DeactivateChosenAddressForCustomerAsync(customerId, (int)address.AddressID);
                    addressesToAdd.Add(newAddress);
                }

                if (!AreAddressesEqual(maillingAddress.Address, newMaillingAddress))
                {
                    await DeactivateChosenAddressForCustomerAsync(customerId, (int)maillingAddress.AddressID);
                    addressesToAdd.Add(newMaillingAddress);
                }

                await AddAddressesForCustomerAsync(customerId, addressesToAdd);
            }
        }
        public async Task DeactivateAllAddressesForCustomerAsync(int customerId)
        {
            var addresses = await context.CustomerAddress
                .Where(x => x.CustomerID == customerId && x.IsActive)
                .ToListAsync();

            foreach (var address in addresses)
            {
                address.IsActive = false;
                address.Address.IsActive = false;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateChosenAddressForCustomerAsync(int customerId, int addressId)
        {
            var address = await context.CustomerAddress
                .Where(x => x.CustomerID == customerId && x.AddressID == addressId && x.IsActive)
                .FirstOrDefaultAsync();

            if (address == null)
            {
                throw new AccountException("Wystąpił błąd z pobieraniem adresu.");
            }

            address.IsActive = false;
            address.Address.IsActive = false;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        private bool AreAddressesEqual(Address address, BaseAddressViewModel newAddress)
        {
            return address.Equals(newAddress);
        }
    }
}
