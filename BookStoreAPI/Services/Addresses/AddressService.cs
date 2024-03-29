﻿using BookStoreAPI.Enums;
using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreData.Models.Orders;
using BookStoreDto.Dtos.Customers.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreAPI.Services.Addresses
{
    public interface IAddressService
    {
        Task<List<Address>> AddAddressesAsync(List<BaseAddressDto> addresses);
        Task<Address> AddAddressesAsync(BaseAddressDto address);
        Task AddAddressesForOrderAsync(int orderId, List<BaseAddressDto> addresses);
        Task AddAddressesForCustomerAsync(int customerId, List<BaseAddressDto> addresses);
        Task UpdateAddressesForCustomerAsync(int customerId, List<BaseAddressDto> newAddresses);
        Task DeactivateAllAddressesForCustomerAsync(int customerId);
        Task DeactivateChosenAddressForCustomerAsync(int customerId, int addressId);
        Task<IEnumerable<BaseAddressDto>> GetCustomerAddressDataAsync(int customerId);
        Task UpdateAddressAsync(int addressId, BaseAddressDto newAddress);
        Task DeactivateAddressAsync(int addressId);
    }
    public class AddressService(BookStoreContext context) : IAddressService
    {
        public async Task<IEnumerable<BaseAddressDto>> GetCustomerAddressDataAsync(int customerId)
        {
            return await context.CustomerAddress
                .Where(x => x.CustomerID == customerId && x.IsActive && 
                        (x.Address.AddressTypeID == (int)AddressTypeEnum.AdresZamieszkania 
                        || x.Address.AddressTypeID == (int)AddressTypeEnum.AdresKorespondencji))
                .OrderBy(x => x.Address.AddressTypeID)
                .Select(x => new BaseAddressDto()
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
        public async Task<List<Address>> AddAddressesAsync(List<BaseAddressDto> addresses)
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
        public async Task<Address> AddAddressesAsync(BaseAddressDto address)
        {
            Address newAddress = new();
            newAddress.CopyProperties(address);

            await context.Address.AddAsync(newAddress);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            return newAddress;
        }
        public async Task AddAddressesForOrderAsync(int orderId, List<BaseAddressDto> addresses)
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

                await context.OrderAddress.AddRangeAsync(orderAddresses);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        public async Task AddAddressesForCustomerAsync(int customerId, List<BaseAddressDto> addresses)
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

                await context.CustomerAddress.AddRangeAsync(customerAddresses);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        public async Task UpdateAddressAsync(int addressId, BaseAddressDto newAddress)
        {
            var oldAddress = await context.Address.Where(x => x.IsActive && x.Id == addressId).FirstOrDefaultAsync();

            if (oldAddress == null)
            {
                throw new BadRequestException("Wystąpił błąd podczas aktualizacji adresu.");
            }

            oldAddress.ModifiedDate = DateTime.UtcNow;
            oldAddress.CopyProperties(newAddress);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task UpdateAddressesForCustomerAsync(int customerId, List<BaseAddressDto> newAddresses)
        {
            if (newAddresses.Any())
            {
                var oldAddresses = await context.CustomerAddress
                    .Include(x => x.Address)
                    .Where(x => x.IsActive && x.CustomerID == customerId && 
                        (x.Address.AddressTypeID == (int)AddressTypeEnum.AdresZamieszkania 
                        || x.Address.AddressTypeID == (int)AddressTypeEnum.AdresKorespondencji))
                    .ToListAsync();

                if (oldAddresses.IsNullOrEmpty())
                {
                    throw new BadRequestException("Wystąpił błąd z aktualizacją adresu.");
                }

                var address = oldAddresses.FirstOrDefault(x => x.Address.AddressTypeID == (int)AddressTypeEnum.AdresZamieszkania);
                var maillingAddress = oldAddresses.FirstOrDefault(x => x.Address.AddressTypeID == (int)AddressTypeEnum.AdresKorespondencji);

                var newAddress = newAddresses.FirstOrDefault(x => x.AddressTypeID == (int)AddressTypeEnum.AdresZamieszkania);
                var newMaillingAddress = newAddresses.FirstOrDefault(x => x.AddressTypeID == (int)AddressTypeEnum.AdresKorespondencji);

                List<BaseAddressDto> addressesToAdd = new();

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
                address.ModifiedDate = DateTime.UtcNow;
                address.Address.IsActive = false;
                address.Address.ModifiedDate = DateTime.UtcNow;
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
            address.ModifiedDate = DateTime.UtcNow;
            address.Address.IsActive = false;
            address.Address.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateAddressAsync(int addressId)
        {
            var address = await context.Address.Where(x => x.IsActive && x.Id == addressId).FirstOrDefaultAsync();
            if (address == null)
            {
                throw new NotFoundException("Wystąpił błąd podczas pobierania adresu do usunięcia.");
            }

            address.ModifiedDate = DateTime.UtcNow;
            address.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        //private bool AreAddressesEqual(Address address, BaseAddressDto newAddress)
        //{
        //    return address.IsEqual(newAddress);
        //}
    }
}
