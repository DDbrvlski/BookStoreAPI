using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Orders;
using BookStoreAPI.Services.Supplies;
using BookStoreBusinessLogic.BusinessLogic.CMS;
using BookStoreData.Data;
using BookStoreData.Models.CMS;
using BookStoreData.Models.Helpers;
using BookStoreDto.Dtos.CMS;
using BookStoreDto.Dtos.Statistics;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.CMS
{
    public interface ICMSService
    {
        Task<CMSWeeklySummaryDto> GetWeeklySummaryOfOrdersRentalsAsync();
    }

    public class CMSService
        (BookStoreContext context) 
        : ICMSService
    {
        public async Task<CMSWeeklySummaryDto> GetWeeklySummaryOfOrdersRentalsAsync()
        {
            return new CMSWeeklySummaryDto()
            {
                NumberOfOrdersThisWeek = await GetNumberOfItemsThisWeek(context.Order),
                NumberOfRentalsThisWeek = await GetNumberOfItemsThisWeek(context.Rental)
            };
        }

        private async Task<int> GetNumberOfItemsThisWeek<T>(DbSet<T> items) where T : BaseEntity
        {
            DateTime currentDate = DateTime.UtcNow;
            int currentDayOfWeek = ((int)currentDate.DayOfWeek == 0) ? 7 : (int)currentDate.DayOfWeek;
            var firstDayOfWeek = currentDate.AddDays(-(currentDayOfWeek - (int)DayOfWeek.Monday));
            var lastDayOfWeek = currentDate.AddDays(7 - currentDayOfWeek);

            return await items
                .Where(x => x.IsActive && x.CreationDate >= firstDayOfWeek
                            && x.CreationDate < lastDayOfWeek)
                .CountAsync();
        }
    }
}
