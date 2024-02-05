using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Orders;
using BookStoreAPI.Services.Supplies;
using BookStoreBusinessLogic.BusinessLogic.CMS;
using BookStoreData.Data;
using BookStoreData.Models.CMS;
using BookStoreData.Models.Orders.Dictionaries;
using BookStoreViewModels.ViewModels.Statistics;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Statistic
{
    public interface IStatisticsService
    {
        Task<StatisticsMonthlyRaportViewModel> GetMonthlyRaportAsync(int month, int year);
    }

    public class StatisticsService
            (BookStoreContext context,
            IOrderService orderService,
            ISupplyService supplyService,
            IStatisticsLogic statisticsLogic) : IStatisticsService
    {
        public async Task<StatisticsMonthlyRaportViewModel> GetMonthlyRaportAsync(int month, int year)
        {
            var statisticsQuery = context.Statistics
                .Where(x => x.IsActive && x.Month == month && x.Year == year)
                .Select(x => new StatisticsMonthlyRaportViewModel()
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
                        .Select(y => new StatisticsBookItemsDetailsViewModel()
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
                        .Select(y => new StatisticsCategoriesDetailsViewModel()
                        {
                            CategoryName = y.Category.Name,
                            NumberOfAppearances = y.NumberOfAppearances,
                            PercentOfTotalAppearances = 0
                        })
                        .ToList()
                });

            var statistics = await statisticsQuery.FirstOrDefaultAsync();

            if (statistics == null)
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
                    Statistics statistics = new();
                    statistics.Month = month;
                    statistics.Year = year;

                    var orderStats = await orderService.GetMonthlyOrderStatisticsAsync(month, year);
                    var supplyStats = await supplyService.GetMonthlySupplyGrossExpensesAsync(month, year);

                    statistics.GrossExpenses = supplyStats.GrossExpenses;
                    statistics.SoldQuantity = orderStats.SoldQuantity;
                    statistics.TotalDiscounts = orderStats.TotalDiscounts;
                    statistics.GrossRevenue = orderStats.GrossRevenue;

                    await context.Statistics.AddAsync(statistics);
                    await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                    await GenerateMonthlyBookItemsInSalesAsync(orderStats.TopBookItemIds, statistics.Id);
                    await GenerateMonthlyCategoriesInSalesAsync(orderStats.TopCategoryIds, statistics.Id);

                    await transaction.CommitAsync();

                    return statistics;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new BadRequestException("Wystąpił błąd podczas generowania miesięcznego raportu.");
                }
            }
        }

        private async Task GenerateMonthlyBookItemsInSalesAsync(List<StatisticsBookItemsViewModel> bookItems, int statisticsId)
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

                await context.BookItemsStatistics.AddRangeAsync(topBookStats);
                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        private async Task GenerateMonthlyCategoriesInSalesAsync(List<StatisticsCategoriesViewModel> categoryIds, int statisticsId)
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

                await context.CategoriesStatistics.AddRangeAsync(topCategoryStats);
                await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            }
        }
        private List<StatisticsBookItemsDetailsViewModel> CalculatePercentageDataInBookStats(List<StatisticsBookItemsDetailsViewModel> bookItems)
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
        private List<StatisticsCategoriesDetailsViewModel> CalculatePercentageDataInCategoryStats(List<StatisticsCategoriesDetailsViewModel> categories)
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
