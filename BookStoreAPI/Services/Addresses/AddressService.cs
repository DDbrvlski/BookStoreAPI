using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreData.Models.Orders;
using BookStoreViewModels.ViewModels.Customers.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreAPI.Services.Addresses
{
    public interface IAddressService
    {
        Task<List<Address>> AddAddressesAsync(List<BaseAddressViewModel> addresses);
        Task AddAddressesForOrderAsync(int orderId, List<BaseAddressViewModel> addresses);
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
                .Where(x => x.CustomerID == customerId && x.IsActive && (x.Address.AddressTypeID == 1 || x.Address.AddressTypeID == 2))
                .OrderBy(x => x.Address.AddressTypeID)
                .Select(x => new BaseAddressViewModel()
                {
                    Id = (int)x.AddressID,
                    AddressTypeID = x.Address.AddressTypeID,
                    CityID = x.Address.CityID,
                    CountryID = x.Address.CountryID,
                    HouseNumber = x.Address.HouseNumber,
                    Postcode = x.Address.Postcode,
                    Street = x.Address.Street,
                    StreetNumber = x.Address.StreetNumber,
                })
                .ToListAsync();
        }
        public async Task<List<Address>> AddAddressesAsync(List<BaseAddressViewModel> addresses)
        {
            var newAddresses = addresses
                    .Where(address => address != null)
                    .Select(address => new Address()
                        .CopyProperties(address))
                    .ToList();

            await context.Address.AddRangeAsync(newAddresses);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            return newAddresses;
        }
        public async Task AddAddressesForOrderAsync(int orderId, List<BaseAddressViewModel> addresses)
        {
            if (addresses.Any())
            {
                var newAddresses = await AddAddressesAsync(addresses);

                var orderAddresses = newAddresses
                    .Select(address => new OrderAddress
                    {
                        OrderID = orderId,
                        AddressID = address.Id
                    })
                    .ToList();

                context.OrderAddress.AddRange(orderAddresses);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        public async Task AddAddressesForCustomerAsync(int customerId, List<BaseAddressViewModel> addresses)
        {
            if (addresses.Any())
            {
                var newAddresses = await AddAddressesAsync(addresses);

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
                    .Include(x => x.Address)
                    .Where(x => x.IsActive && x.CustomerID == customerId && (x.Address.AddressTypeID == 1 || x.Address.AddressTypeID == 2))
                    .ToListAsync();

                if (oldAddresses.IsNullOrEmpty())
                {
                    throw new BadRequestException("Wystąpił błąd z aktualizacją adresu.");
                }

                var address = oldAddresses.FirstOrDefault(x => x.Address.AddressTypeID == 1);
                var maillingAddress = oldAddresses.FirstOrDefault(x => x.Address.AddressTypeID == 2);

                var newAddress = newAddresses.FirstOrDefault(x => x.AddressTypeID == 1);
                var newMaillingAddress = newAddresses.FirstOrDefault(x => x.AddressTypeID == 2);

                List<BaseAddressViewModel> addressesToAdd = new();

                if (address == null)
                {
                    addressesToAdd.Add(newAddress);
                }
                else if (!address.Address.IsEqual(newAddress))
                {
                    await DeactivateChosenAddressForCustomerAsync(customerId, (int)address.AddressID);
                    addressesToAdd.Add(newAddress);
                }

                if (maillingAddress == null)
                {
                    addressesToAdd.Add(newMaillingAddress);
                }
                else if (!maillingAddress.Address.IsEqual(newMaillingAddress))
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
        //private bool AreAddressesEqual(Address address, BaseAddressViewModel newAddress)
        //{
        //    return address.IsEqual(newAddress);
        //}
    }
}
