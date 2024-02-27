using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Orders;
using BookStoreAPI.Services.Supplies;
using BookStoreBusinessLogic.BusinessLogic.CMS;
using BookStoreData.Data;
using BookStoreData.Models.CMS;
using BookStoreDto.Dtos.Statistics;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Statistic
{
    public interface IStatisticsService
    {
        Task<StatisticsMonthlyRaportDto> GetMonthlyRaportAsync(int month, int year);
    }

    public class StatisticsService
            (BookStoreContext context,
            IOrderService orderService,
            ISupplyService supplyService,
            IStatisticsLogic statisticsLogic) : IStatisticsService
    {
        public async Task<StatisticsMonthlyRaportDto> GetMonthlyRaportAsync(int month, int year)
        {
            var statisticsQuery = context.Statistics
                .Where(x => x.IsActive && x.Month == month && x.Year == year)
                .Select(x => new StatisticsMonthlyRaportDto()
                {
                    Month = month,
                    Year = year,
                    GrossExpenses = x.GrossExpenses,
                    GrossRevenue = x.GrossRevenue,
                    SoldQuantity = x.SoldQuantity,
                    TotalDiscounts = x.TotalDiscounts,
                    TotalIncome = 0,
                    BookItems = x.BookItemsStatistics
                        .Where(y => y.IsActive)
                        .Select(y => new StatisticsBookItemsDetailsDto()
                        {
                            BookTitle = y.BookItem.Book.Title,
                            FormTitle = y.BookItem.Form.Name,
                            PercentOfTotalSoldPrice = 0,
                            PercentOfTotalSoldUnits = 0,
                            SoldPrice = y.SoldPrice,
                            SoldUnits = y.SoldQuantity
                        })
                        .ToList(),
                    Categories = x.CategoriesStatistics
                        .Where(y => y.IsActive)
                        .Select(y => new StatisticsCategoriesDetailsDto()
                        {
                            CategoryName = y.Category.Name,
                            NumberOfAppearances = y.NumberOfAppearances,
                            PercentOfTotalAppearances = 0
                        })
                        .ToList()
                });

            var statistics = await statisticsQuery.FirstOrDefaultAsync();

            if (statistics == null || statistics.Month == DateTime.Now.Month)
            {
                await GenerateMonthlyRaportAsync(month, year);
                statistics = await statisticsQuery.FirstOrDefaultAsync();
            }

            statistics.BookItems = CalculatePercentageDataInBookStats(statistics.BookItems);
            statistics.Categories = CalculatePercentageDataInCategoryStats(statistics.Categories);
            statistics.TotalIncome = statistics.GrossRevenue - statistics.GrossExpenses;

            return statistics;
        }

        private async Task<Statistics> GenerateMonthlyRaportAsync(int month, int year)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    bool isUpdate = false;
                    var statisticsDB = await context.Statistics
                        .Where(x => x.IsActive && x.Month == month && x.Year == year)
                        .FirstOrDefaultAsync();
                    Statistics statistics = new();
                    if (statisticsDB != null)
                    {
                        isUpdate = true;
                        statistics = statisticsDB;
                    }
                    
                    statistics.Month = month;
                    statistics.Year = year;

                    var orderStats = await orderService.GetMonthlyOrderStatisticsAsync(month, year);
                    var supplyStats = await supplyService.GetMonthlySupplyGrossExpensesAsync(month, year);

                    statistics.GrossExpenses = supplyStats.GrossExpenses;
                    statistics.SoldQuantity = orderStats.SoldQuantity;
                    statistics.TotalDiscounts = orderStats.TotalDiscounts;
                    statistics.GrossRevenue = orderStats.GrossRevenue;

                    if (!isUpdate)
                    {
                        await context.Statistics.AddAsync(statistics);
                    }
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    await GenerateMonthlyBookItemsInSalesAsync(orderStats.TopBookItemIds, statistics.Id, isUpdate);
                    await GenerateMonthlyCategoriesInSalesAsync(orderStats.TopCategoryIds, statistics.Id, isUpdate);

                    await transaction.CommitAsync();

                    return statistics;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task GenerateMonthlyBookItemsInSalesAsync(List<StatisticsBookItemsDto> bookItems, int statisticsId, bool isUpdate)
        {
            if (bookItems.Any())
            {
                List<BookItemsStatistics> topBookStats = new();
                foreach (var item in bookItems)
                {
                    topBookStats.Add(new BookItemsStatistics()
                    {
                        SoldQuantity = item.SoldQuantity,
                        BookItemID = item.BookItemId,
                        StatisticsID = statisticsId,
                        SoldPrice = item.SoldPrice,
                    });
                }

                if (!isUpdate)
                {
                    await context.BookItemsStatistics.AddRangeAsync(topBookStats);
                }
                else
                {
                    var existingTopBookStats = await context.BookItemsStatistics
                        .Where(x => x.IsActive && x.StatisticsID == statisticsId)
                        .ToListAsync();

                    var updateTopBookStats = existingTopBookStats.Where(x => topBookStats.Any(y => y.BookItemID == x.BookItemID && y.SoldQuantity != x.SoldQuantity && y.SoldPrice != x.SoldPrice)).ToList();
                    var newTopBookStats = topBookStats.Where(x => !existingTopBookStats.Any(y => y.BookItemID == x.BookItemID)).ToList();

                    foreach (var book in topBookStats)
                    {
                        var bookStatToUpdate = updateTopBookStats.Find(x => x.BookItemID == book.BookItemID);
                        if (bookStatToUpdate != null)
                        {
                            bookStatToUpdate.SoldPrice = book.SoldPrice;
                            bookStatToUpdate.SoldQuantity = book.SoldQuantity;
                        }
                    }

                    await context.BookItemsStatistics.AddRangeAsync(newTopBookStats);
                }
                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        private async Task GenerateMonthlyCategoriesInSalesAsync(List<StatisticsCategoriesDto> categoryIds, int statisticsId, bool isUpdate)
        {
            if (categoryIds.Any())
            {
                List<CategoriesStatistics> topCategoryStats = new();
                foreach (var item in categoryIds)
                {
                    topCategoryStats.Add(new CategoriesStatistics()
                    {
                        CategoryID = item.CategoryId,
                        StatisticsID = statisticsId,
                        NumberOfAppearances = item.NumberOfAppearances,
                    });
                }

                if (!isUpdate)
                {                    
                    await context.CategoriesStatistics.AddRangeAsync(topCategoryStats);
                }
                else
                {
                    var existingTopCategoryStats = await context.CategoriesStatistics
                        .Where(x => x.IsActive && x.StatisticsID == statisticsId)
                        .ToListAsync();

                    var updateTopCategoryStats = existingTopCategoryStats.Where(x => topCategoryStats.Any(y => y.CategoryID == x.CategoryID && y.NumberOfAppearances != x.NumberOfAppearances)).ToList();
                    var newTopCategoryStats = topCategoryStats.Where(x => !existingTopCategoryStats.Any(y => y.CategoryID == x.CategoryID)).ToList();

                    foreach (var category in topCategoryStats)
                    {
                        var categoryStatToUpdate = updateTopCategoryStats.Find(x => x.CategoryID == category.CategoryID);
                        if (categoryStatToUpdate != null)
                        {
                            categoryStatToUpdate.NumberOfAppearances = category.NumberOfAppearances;
                        }
                    }

                    await context.CategoriesStatistics.AddRangeAsync(newTopCategoryStats);
                }
                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        private List<StatisticsBookItemsDetailsDto> CalculatePercentageDataInBookStats(List<StatisticsBookItemsDetailsDto> bookItems)
        {
            int totalSoldUnits = bookItems.Select(x => x.SoldUnits).Sum();
            decimal totalSoldPrices = bookItems.Select(x => x.SoldPrice).Sum();

            foreach (var item in bookItems)
            {
                item.PercentOfTotalSoldPrice = statisticsLogic.PercentNumberOfPrices(item.SoldPrice, totalSoldPrices);
                item.PercentOfTotalSoldUnits = statisticsLogic.PercentNumberOfAppearances(item.SoldUnits, totalSoldUnits);
            }

            return bookItems;
        }
        private List<StatisticsCategoriesDetailsDto> CalculatePercentageDataInCategoryStats(List<StatisticsCategoriesDetailsDto> categories)
        {
            int totalNumberOfAppearances = categories.Select(x => x.NumberOfAppearances).Sum();

            foreach (var item in categories)
            {
                item.PercentOfTotalAppearances = statisticsLogic.PercentNumberOfAppearances(item.NumberOfAppearances, totalNumberOfAppearances);
            }

            return categories;
        }
    }
}
