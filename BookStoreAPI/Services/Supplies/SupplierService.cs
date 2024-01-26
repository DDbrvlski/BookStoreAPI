using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Addresses;
using BookStoreData.Data;
using BookStoreData.Models.Supplies.Dictionaries;
using BookStoreViewModels.ViewModels.Customers.Address;
using BookStoreViewModels.ViewModels.Supply;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreAPI.Services.Supplies
{
    public interface ISupplierService
    {
        Task AddNewSupplierAsync(SupplierPostViewModel supplierData);
        Task<SupplierViewModel> GetSuppliersDataAsync(int supplierId);
        Task<IEnumerable<SupplierShortViewModel>> GetSuppliersShortDataAsync();
        Task UpdateSupplierAsync(int supplierId, SupplierPostViewModel supplierData);
        Task DeactivateSupplierAsync(int supplierId);
    }

    public class SupplierService(BookStoreContext context, IAddressService addressService) : ISupplierService
    {
        public async Task AddNewSupplierAsync(SupplierPostViewModel supplierData)
        {
            var address = await addressService.AddAddressesAsync(new List<BaseAddressViewModel> { supplierData.Address });

            if (address.IsNullOrEmpty())
            {
                throw new BadRequestException("Wystąpił błąd.");
            }

            Supplier supplier = new Supplier();
            supplier.CopyProperties(supplierData);

            supplier.AddressID = address.First().Id;

            await context.Supplier.AddAsync(supplier);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task UpdateSupplierAsync(int supplierId, SupplierPostViewModel supplierData)
        {
            var address = await context.Address.Where(x => x.IsActive && x.Id == supplierData.Address.Id).FirstOrDefaultAsync();

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
        }

        public async Task<IEnumerable<SupplierShortViewModel>> GetSuppliersShortDataAsync()
        {
            return await context.Supplier
                .Where(x => x.IsActive)
                .Select(x => new SupplierShortViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task DeactivateSupplierAsync(int supplierId)
        {
            var supplier = await context.Supplier.Where(x => x.IsActive && x.Id == supplierId).FirstOrDefaultAsync();
            if (supplier == null)
            {
                throw new NotFoundException("Wystąpił błąd podczas pobierania dostawcy do usunięcia.");
            }
            await addressService.DeactivateAddressAsync((int)supplier.AddressID);

            supplier.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task<SupplierViewModel> GetSuppliersDataAsync(int supplierId)
        {
            var supplierDetails = await context.Supplier
                .Where(x => x.IsActive && x.Id == supplierId)
                .Select(x => new SupplierViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    SupplierAddress = new AddressDetailsViewModel()
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
