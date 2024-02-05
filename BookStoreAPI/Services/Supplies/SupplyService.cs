using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Payments;
using BookStoreData.Data;
using BookStoreData.Models.Supplies;
using BookStoreViewModels.ViewModels.Customers.Address;
using BookStoreViewModels.ViewModels.Payments;
using BookStoreViewModels.ViewModels.Payments.Dictionaries;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;
using BookStoreViewModels.ViewModels.Supply;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreAPI.Services.Supplies
{
    public interface ISupplyService
    {
        Task<IEnumerable<SupplyViewModel>> GetAllSuppliesAsync();
        Task<SupplyDetailsViewModel> GetSupplyAsync(int supplyId);
        Task AddNewSupplyAsync(SupplyPostViewModel supplyData);
        Task DeactivateSupplyAsync(int supplyId);
        Task UpdateSupplyAsync(int supplyId, SupplyPutViewModel supplyData);
        Task<SupplyStatisticsViewModel> GetMonthlySupplyGrossExpensesAsync(int month, int year);
    }

    public class SupplyService
            (BookStoreContext context,
            ISupplyGoodsService supplyGoodsService,
            IPaymentService paymentService) : ISupplyService
    {
        public async Task<IEnumerable<SupplyViewModel>> GetAllSuppliesAsync()
        {
            return await context.Supply
                .Where(x => x.IsActive)
                .Select(x => new SupplyViewModel()
                {
                    Id = x.Id,
                    SupplierName = x.Supplier.Name,
                    SupplyDate = x.CreationDate,
                    PriceBrutto = x.SupplyGoods
                                .Where(y => y.IsActive && y.SupplyID == x.Id)
                                .Select(y => y.BruttoPrice * y.Quantity)
                                .Sum()
                })
                .ToListAsync();
        }
        public async Task<SupplyDetailsViewModel> GetSupplyAsync(int supplyId)
        {
            var supply = await context.Supply
                .Where(x => x.IsActive && x.Id == supplyId)
                .Select(x => new SupplyDetailsViewModel()
                {
                    Id = x.Id,
                    DeliveryDate = x.DeliveryDate,
                    DeliveryStatusId = (int)x.DeliveryStatusID,
                    DeliveryStatusName = x.DeliveryStatus.Name,
                    SupplierData = new SupplierViewModel()
                    {
                        Id = x.Supplier.Id,
                        Name = x.Supplier.Name,
                        Email = x.Supplier.Email,
                        PhoneNumber = x.Supplier.PhoneNumber,
                        SupplierAddress = new AddressDetailsViewModel()
                        {
                            Id = x.Supplier.Address.Id,
                            AddressTypeID = x.Supplier.Address.AddressTypeID,
                            CityID = x.Supplier.Address.CityID,
                            CityName = x.Supplier.Address.City.Name,
                            CountryID = x.Supplier.Address.CountryID,
                            CountryName = x.Supplier.Address.Country.Name,
                            HouseNumber = x.Supplier.Address.HouseNumber,
                            Postcode = x.Supplier.Address.Postcode,
                            Street = x.Supplier.Address.Street,
                            StreetNumber = x.Supplier.Address.StreetNumber,
                        }
                    },
                    PaymentData = new PaymentDetailsViewModel()
                    {
                        Id = x.Payment.Id,
                        Amount = x.Payment.Amount,
                        PaymentDate = x.Payment.PaymentDate,
                        PaymentMethod = new PaymentMethodViewModel()
                        {
                            Id = x.Payment.PaymentMethod.Id,
                            Name = x.Payment.PaymentMethod.Name
                        },
                        TransactionStatus = new TransactionStatusViewModel()
                        {
                            Id = x.Payment.TransactionStatus.Id,
                            Name = x.Payment.TransactionStatus.Name
                        },
                    },
                    SupplyBooksData = x.SupplyGoods
                    .Where(y => y.IsActive && y.SupplyID == supplyId)
                    .Select(y => new SupplyBooksViewModel()
                    {
                        BookItemId = y.BookItemID,
                        BookTitle = y.BookItem.Book.Title,
                        EditionName = y.BookItem.Edition.Name,
                        FileFormatName = y.BookItem.FileFormat.Name,
                        FormName = y.BookItem.Form.Name,
                        BruttoPrice = y.BruttoPrice,
                        Quantity = y.Quantity,
                        Authors = y.BookItem.Book.BookAuthors
                            .Where(y => y.IsActive)
                            .Select(y => new AuthorViewModel()
                            {
                                Id = (int)y.AuthorID,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if(supply == null)
            {
                throw new NotFoundException("Nie znaleziono podanej dostawy.");
            }

            return supply;
        }
        public async Task AddNewSupplyAsync(SupplyPostViewModel supplyData)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    decimal paymentAmount = 0;
                    if (!supplyData.BookItems.IsNullOrEmpty())
                    {
                        paymentAmount = supplyData.BookItems.Select(x => x.BruttoPrice * (int)(x.Quantity ?? 1)).Sum();
                    }

                    var payment = await paymentService.CreateNewPayment(supplyData.PaymentMethodId, paymentAmount);

                    Supply supply = new Supply();
                    supply.SupplierID = supplyData.SupplierId;
                    if (supplyData.DeliveryDate != default)
                    {
                        supply.DeliveryDate = supplyData.DeliveryDate;
                    }
                    supply.DeliveryStatusID = supplyData.DeliveryStatusId;
                    supply.PaymentID = payment.Id;
                    await context.Supply.AddAsync(supply);

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    await supplyGoodsService.AddNewSupplyBooksAsync(supply.Id, supplyData.BookItems);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new BadRequestException("Wystąpił błąd podczas dodawania nowej dostawy.");
                }
            }
        }
        public async Task UpdateSupplyAsync(int supplyId, SupplyPutViewModel supplyData)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var supply = await context.Supply.Where(x => x.IsActive && x.Id == supplyId).FirstOrDefaultAsync();
                    if (supply == null)
                    {
                        throw new BadRequestException("Wystąpił błąd podczas pobierania dostawy.");
                    }

                    supply.DeliveryDate = supplyData.DeliveryDate;
                    supply.DeliveryStatusID = supplyData.DeliveryStatusId;
                    supply.SupplierID = supplyData.SupplierId;

                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    await supplyGoodsService.UpdateSupplyBooksAsync(supplyId, supplyData.BookItems);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new BadRequestException("Wystąpił błąd podczas aktualizowania dostawy.");
                }
            }
        }
        public async Task DeactivateSupplyAsync(int supplyId)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var supply = await context.Supply.Where(x => x.IsActive && x.Id == supplyId).FirstOrDefaultAsync();
                    if (supply == null)
                    {
                        throw new BadRequestException("Wystąpił błąd podczas pobierania dostawy.");
                    }

                    supply.IsActive = false;
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    await supplyGoodsService.DeactivateAllSupplyGoodsAsync(supplyId);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new BadRequestException("Wystąpił błąd podczas usuwania dostawy.");
                }
            }
        }
        public async Task<SupplyStatisticsViewModel> GetMonthlySupplyGrossExpensesAsync(int month, int year)
        {
            var supplyStats = await context.SupplyGoods
                .Where(x => x.IsActive && x.CreationDate.Month == month && x.CreationDate.Year == year)
                .GroupBy(x => 1)
                .Select(group => new SupplyStatisticsViewModel()
                {
                    GrossExpenses = group.Sum(y => y.BruttoPrice * y.Quantity)
                })
                .FirstOrDefaultAsync();

            return supplyStats ?? new SupplyStatisticsViewModel();
        }
    }
}
