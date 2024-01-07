using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Users;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;
using BookStoreViewModels.ViewModels.Products.Reservations;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BookStoreAPI.Services.Reservation
{
    public interface IReservationService
    {
        Task DeactivateReservationAsync(int bookItemId);
        Task<IEnumerable<ReservationsMadeByAllCustomersCMSViewModel>> GetAllReservationsMadeByCustomersCMSAsync();
        Task<ReservationsMadeByCustomerCMSViewModel> GetAllReservationsMadeBySingleCustomerByIdCMSAsync(int customerId);
        Task<IEnumerable<ReservationViewModel>> GetUserAllReservationsAsync();
        Task ReservateABookItemAsync(int bookItemId);
    }

    public class ReservationService(BookStoreContext context, IUserContextService userContextService) : IReservationService
    {
        public async Task ReservateABookItemAsync(int bookItemId)
        {
            var user = await userContextService.GetUserByTokenAsync();

            if (user == null)
            {
                throw new UnauthorizedException("Należy się zalogować.");
            }

            var isAvailableToReservate = await context.BookItem.Where(x => x.IsActive && x.Id == bookItemId).Select(x => x.AvailabilityID).FirstAsync();
            if (isAvailableToReservate == 1)
            {
                throw new BadRequestException("Nie można zarezerwować dostępnego produktu.");
            }

            var lastReservationNumber = await context.Reservations.OrderBy(x => x.Position).LastOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);
            int nextPositionInReservationQueue = 1;

            if (lastReservationNumber != null)
            {
                nextPositionInReservationQueue = lastReservationNumber.Position;
            }

            var alreadyExistingReservation = await context.Reservations.FirstOrDefaultAsync(x => !x.IsActive && x.BookItemID == bookItemId);
            if (alreadyExistingReservation != null)
            {
                alreadyExistingReservation.IsActive = true;
                alreadyExistingReservation.Position = nextPositionInReservationQueue;
            }
            else
            {
                Reservations reservation = new Reservations()
                {
                    Position = nextPositionInReservationQueue,
                    BookItemID = bookItemId,
                    UserID = user.Id
                };
                await context.AddAsync(reservation);
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task DeactivateReservationAsync(int bookItemId)
        {
            var user = await userContextService.GetUserByTokenAsync();

            if (user == null)
            {
                throw new UnauthorizedException("Należy się zalogować.");
            }

            var reservation = await context.Reservations.FirstOrDefaultAsync(x => x.IsActive && x.UserID == user.Id && x.BookItemID == bookItemId);

            if (reservation == null)
            {
                throw new NotFoundException("Nie znaleziono rezerwacji.");
            }

            await UpdateReservationPositionInQueue(reservation.Position, bookItemId);
            reservation.IsActive = false;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        private async Task UpdateReservationPositionInQueue(int position, int bookItemId)
        {
            var reservations = await context.Reservations
                .OrderBy(x => x.Position)
                .Where(x => x.Position > position && x.BookItemID == bookItemId && x.IsActive)
                .ToListAsync();

            foreach (var item in reservations)
            {
                item.Position--;
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }

        public async Task<IEnumerable<ReservationViewModel>> GetUserAllReservationsAsync()
        {
            var user = await userContextService.GetUserByTokenAsync();

            if (user == null)
            {
                throw new UnauthorizedException("Należy się zalogować.");
            }

            return await context.Reservations
                .Where(x => x.IsActive && x.UserID == user.Id)
                .Select(x => new ReservationViewModel()
                {
                    Id = x.Id,
                    PositionInQueue = x.Position,
                    Item = new ReservationBookItemViewModel()
                    {
                        Id = x.BookItem.Id,
                        Title = x.BookItem.Book.Title,
                        ImageURL = x.BookItem.Book.BookImages.Where(x => x.IsActive && x.Image.Position == 1).Select(x => x.Image.ImageURL).First(),
                        Price = x.BookItem.NettoPrice * (1 + (decimal)(x.BookItem.Tax / 100.0f)),
                        Score = x.BookItem.Score,
                        FormName = x.BookItem.Form.Name,
                        FileFormatName = x.BookItem.FileFormat.Name,
                        EditionName = x.BookItem.Edition.Name,
                        Authors = x.BookItem.Book.BookAuthors.Where(x => x.IsActive).Select(y => new AuthorViewModel
                        {
                            Id = (int)y.AuthorID,
                            Name = y.Author.Name,
                            Surname = y.Author.Surname,
                        }).ToList(),
                    }
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReservationsMadeByAllCustomersCMSViewModel>> GetAllReservationsMadeByCustomersCMSAsync()
        {
            return await context.Reservations
                .Where(x => x.IsActive)
                .Select(x => new ReservationsMadeByAllCustomersCMSViewModel()
                {
                    CustomerId = (int)x.User.CustomerID,
                    CustomerEmail = x.User.Email,
                    NumberOfReservations = context.Reservations
                                    .Count(r => r.IsActive && r.UserID == x.UserID)
                })
                .ToListAsync();
        }

        public async Task<ReservationsMadeByCustomerCMSViewModel> GetAllReservationsMadeBySingleCustomerByIdCMSAsync(int customerId)
        {
            var userReservations = await context.Reservations
                .Where(x => x.IsActive && x.User.CustomerID == customerId)
                .Select(x => new ReservationViewModel()
                {
                    Id = x.Id,
                    PositionInQueue = x.Position,
                    Item = new ReservationBookItemViewModel()
                    {
                        Id = x.BookItem.Id,
                        Title = x.BookItem.Book.Title,
                        ImageURL = x.BookItem.Book.BookImages.Where(x => x.IsActive && x.Image.Position == 1).Select(x => x.Image.ImageURL).First(),
                        Price = x.BookItem.NettoPrice * (1 + (decimal)(x.BookItem.Tax / 100.0f)),
                        Score = x.BookItem.Score,
                        FormName = x.BookItem.Form.Name,
                        FileFormatName = x.BookItem.FileFormat.Name,
                        EditionName = x.BookItem.Edition.Name,
                        Authors = x.BookItem.Book.BookAuthors.Where(x => x.IsActive).Select(y => new AuthorViewModel
                        {
                            Id = (int)y.AuthorID,
                            Name = y.Author.Name,
                            Surname = y.Author.Surname,
                        }).ToList(),
                    }
                })
                .ToListAsync();

            return new ReservationsMadeByCustomerCMSViewModel()
            {
                CustomerId = customerId,
                ReservationList = userReservations
            };
        }
    }
}
