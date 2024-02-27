using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Customers;
using BookStoreData.Data;
using BookStoreData.Models.Rental;
using BookStoreDto.Dtos.Rentals;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Rentals
{
    public interface IRentalService
    {
        Task CreateRentalAsync(RentalPostDto rentalModel);
        Task<IEnumerable<RentalDto>> GetUserRentalsByRentalStatusIdAsync(int rentalTypeId);
    }

    public class RentalService(BookStoreContext context, ICustomerService customerService) : IRentalService
    {
        public async Task<IEnumerable<RentalDto>> GetUserRentalsByRentalStatusIdAsync(int rentalStatusId)
        {
            var customer = await customerService.GetCustomerByTokenAsync();

            var rentals = context.Rental.AsQueryable();
                
            if (rentalStatusId != 0)
            {
                 rentals = rentals.Where(x => x.IsActive && x.CustomerID == customer.Id && x.RentalStatusID == rentalStatusId);
            }
            else
            {
                rentals = rentals.Where(x => x.IsActive && x.CustomerID == customer.Id);
            }

            return await rentals.Select(x => new RentalDto()
            {
                Id = x.Id,
                BookItemId = x.BookItemID,
                BookTitle = x.BookItem.Book.Title,
                ExpirationDate = x.EndDate,
                FileFormatName = x.BookItem.FileFormat.Name,
                ImageURL = x.BookItem.Book.BookImages
                        .FirstOrDefault(y => y.IsActive && y.Image.Position == 1).Image.ImageURL
            }).ToListAsync();

        }
        public async Task CreateRentalAsync(RentalPostDto rentalModel)
        {
            var customer = await customerService.GetCustomerByTokenAsync();
            var days = await context.RentalType
                .Where(x => x.IsActive && x.Id == rentalModel.RentalTypeID)
                .Select(x => x.Days)
                .FirstOrDefaultAsync();

            Rental rental = new Rental();
            rental.CopyProperties(rentalModel);
            rental.CustomerID = customer.Id;
            rental.RentalStatusID = 1;
            if (days == 0)
            {
                rental.EndDate = new DateTime(2100, 1, 1);
            }
            else
            {
                rental.EndDate = rental.StartDate.AddDays(days);
            }

            await context.Rental.AddAsync(rental);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
