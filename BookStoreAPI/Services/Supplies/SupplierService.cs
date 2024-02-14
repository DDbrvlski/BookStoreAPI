using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Addresses;
using BookStoreData.Data;
using BookStoreData.Models.Supplies.Dictionaries;
using BookStoreDto.Dtos.Customers.Address;
using BookStoreDto.Dtos.Supply;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreAPI.Services.Supplies
{
    public interface ISupplierService
    {
        Task AddNewSupplierAsync(SupplierPostDto supplierData);
        Task<SupplierDto> GetSuppliersDataAsync(int supplierId);
        Task<IEnumerable<SupplierShortDto>> GetSuppliersShortDataAsync();
        Task UpdateSupplierAsync(int supplierId, SupplierPostDto supplierData);
        Task DeactivateSupplierAsync(int supplierId);
    }

    public class SupplierService(BookStoreContext context, IAddressService addressService) : ISupplierService
    {
        public async Task AddNewSupplierAsync(SupplierPostDto supplierData)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var address = await addressService.AddAddressesAsync(new List<BaseAddressDto> { supplierData.Address });

                    if (address.IsNullOrEmpty())
                    {
                        throw new BadRequestException("Wystąpił błąd.");
                    }

                    Supplier supplier = new Supplier();
                    supplier.CopyProperties(supplierData);

                    supplier.AddressID = address.First().Id;

                    await context.Supplier.AddAsync(supplier);
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
            
        }

        public async Task UpdateSupplierAsync(int supplierId, SupplierPostDto supplierData)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var address = await context.Supplier.Where(x => x.IsActive && x.Id == supplierId).Select(x => x.Address).FirstOrDefaultAsync();

                    if (address != null && !supplierData.Address.IsEqual(address))
                    {
                        await addressService.UpdateAddressAsync(address.Id, supplierData.Address);
                    }
                    else if (address == null)
                    {
                        throw new NotFoundException("Wystąpił błąd podczas pobierania adresu do aktualizacji.");
                    }

                    var supplier = await context.Supplier.Where(x => x.IsActive && x.Id == supplierId).FirstOrDefaultAsync();

                    if (supplier == null)
                    {
                        throw new NotFoundException("Wystąpił bład podczas pobierania dostawcy do aktualizacji");
                    }

                    supplier.CopyProperties(supplierData);
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
            
        }

        public async Task<IEnumerable<SupplierShortDto>> GetSuppliersShortDataAsync()
        {
            return await context.Supplier
                .Where(x => x.IsActive)
                .Select(x => new SupplierShortDto()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task DeactivateSupplierAsync(int supplierId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var supplier = await context.Supplier.Where(x => x.IsActive && x.Id == supplierId).FirstOrDefaultAsync();
                    if (supplier == null)
                    {
                        throw new NotFoundException("Wystąpił błąd podczas pobierania dostawcy do usunięcia.");
                    }
                    await addressService.DeactivateAddressAsync((int)supplier.AddressID);

                    supplier.IsActive = false;
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
            
        }

        public async Task<SupplierDto> GetSuppliersDataAsync(int supplierId)
        {
            var supplierDetails = await context.Supplier
                .Where(x => x.IsActive && x.Id == supplierId)
                .Select(x => new SupplierDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    AddressTypeName = x.Address.AddressType.Name,
                    SupplierAddress = new AddressDetailsDto()
                    {
                        Id = x.Address.Id,
                        AddressTypeID = x.Address.AddressTypeID,
                        CityID = x.Address.CityID,
                        CityName = x.Address.City.Name,
                        CountryID = x.Address.CountryID,
                        CountryName = x.Address.Country.Name,
                        HouseNumber = x.Address.HouseNumber,
                        Postcode = x.Address.Postcode,
                        Street = x.Address.Street,
                        StreetNumber = x.Address.StreetNumber,
                    }
                })
                .FirstOrDefaultAsync();

            if (supplierDetails == null)
            {
                throw new NotFoundException("Nie znaleziono danych dostawcy.");
            }

            return supplierDetails;
        }
    }
}
