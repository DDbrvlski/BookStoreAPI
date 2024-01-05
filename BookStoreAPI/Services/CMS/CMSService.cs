using BookStoreData.Data;
using BookStoreData.Models.Helpers;
using BookStoreViewModels.ViewModels.CMS;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.CMS
{
    public interface ICMSService
    {
        Task<CMSWeeklySummaryViewModel> GetWeeklySummaryOfOrdersRentalsReservationsAsync();
    }

    public class CMSService(BookStoreContext context) : ICMSService
    {
        public async Task<CMSWeeklySummaryViewModel> GetWeeklySummaryOfOrdersRentalsReservationsAsync()
        {
            return new CMSWeeklySummaryViewModel()
            {
                NumberOfOrdersThisWeek = await GetNumberOfItemsThisWeek(context.Order),
                NumberOfRentalsThisWeek = await GetNumberOfItemsThisWeek(context.Rental),
                NumberOfReservationsThisWeek = 0
            };
        }

        private async Task<int> GetNumberOfItemsThisWeek<T>(DbSet<T> items) where T : BaseEntity
        {
            DateTime currentDate = DateTime.UtcNow;

            return await items
                .Where(x => x.IsActive && x.CreationDate >= currentDate.AddDays(-((int)currentDate.DayOfWeek - (int)DayOfWeek.Monday))
                            && x.CreationDate < currentDate.AddDays(7 - (int)currentDate.DayOfWeek))
                .CountAsync();
        }
    }
}
