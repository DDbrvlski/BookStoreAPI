using BookStoreData.Models.Orders;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Orders;
using BookStoreDto.Dtos.Products.BookItems;

namespace BookStoreAPI.Helpers
{
    public static class FiltersExtensions
    {
        #region BookItemFilters
        public static IQueryable<BookItem> ApplyBookFilters(this IQueryable<BookItem> query, BookItemFiltersDto filters)
        {
            if (filters == null)
                return query;

            if (!string.IsNullOrEmpty(filters.SearchPhrase))
                query = query.SearchBy(filters.SearchPhrase);

            if (filters.AuthorIds != null && filters.AuthorIds.Any())
                query = query.WhereHasAuthors(filters.AuthorIds);

            if (filters.CategoryIds != null && filters.CategoryIds.Any())
                query = query.WhereHasCategories(filters.CategoryIds);

            if (filters.FormIds != null && filters.FormIds.Any())
                query = query.WhereHasForms(filters.FormIds);

            if (filters.PublisherIds != null && filters.PublisherIds.Any())
                query = query.WhereHasPublishers(filters.PublisherIds);

            if (filters.LanguageIds != null && filters.LanguageIds.Any())
                query = query.WhereHasLanguages(filters.LanguageIds);

            if (filters.ScoreValues != null && filters.ScoreValues.Any())
                query = query.WhereHasScores(filters.ScoreValues);

            if (filters.AvailabilitiesIds != null && filters.AvailabilitiesIds.Any())
                query = query.WhereHasAvailabilities(filters.AvailabilitiesIds);

            if (filters.BookId != null)
                query = query.WhereHasBook(filters.BookId);

            if (filters.PriceFrom != null)
                query = query.WherePriceFrom(filters.PriceFrom);

            if (filters.PriceTo != null)
                query = query.WherePriceTo(filters.PriceTo);

            if (filters.IsOnSale != null)
                query = query.WhereIsOnSale(filters.IsOnSale);

            if (!string.IsNullOrEmpty(filters.SortBy) && !string.IsNullOrEmpty(filters.SortOrder))
                query = query.OrderBy(filters.SortBy, filters.SortOrder);

            if (filters.NumberOfElements != null)
                query = query.WhereNumberOfElements(filters.NumberOfElements);

            return query;
        }

        public static IQueryable<BookItem> WhereHasAuthors(this IQueryable<BookItem> query, List<int?> authorIds)
        {
            return query.Where(x => x.Book.BookAuthors.Any(a => authorIds.Contains(a.AuthorID)));
        }

        public static IQueryable<BookItem> WhereHasCategories(this IQueryable<BookItem> query, List<int?> categoryIds)
        {
            return query.Where(x => x.Book.BookCategories.Any(a => categoryIds.Contains(a.CategoryID)));
        }

        public static IQueryable<BookItem> WhereHasForms(this IQueryable<BookItem> query, List<int?> formIds)
        {
            return query.Where(x => formIds.Contains(x.FormID));
        }

        public static IQueryable<BookItem> WhereHasPublishers(this IQueryable<BookItem> query, List<int?> publisherIds)
        {
            return query.Where(x => publisherIds.Contains(x.Book.PublisherID));
        }

        public static IQueryable<BookItem> WhereHasLanguages(this IQueryable<BookItem> query, List<int?> languageIds)
        {
            return query.Where(x => languageIds.Contains(x.Book.OriginalLanguageID));
        }

        public static IQueryable<BookItem> WhereHasScores(this IQueryable<BookItem> query, List<int?> scoreValues)
        {
            return query.Where(x => scoreValues.Contains((int)x.Score));
        }

        public static IQueryable<BookItem> WhereHasAvailabilities(this IQueryable<BookItem> query, List<int?> availabilitiesIds)
        {
            return query.Where(x => availabilitiesIds.Contains(x.AvailabilityID));
        }

        public static IQueryable<BookItem> WhereHasBook(this IQueryable<BookItem> query, int? bookId)
        {
            return query.Where(x => x.BookID == bookId);
        }

        public static IQueryable<BookItem> WherePriceFrom(this IQueryable<BookItem> query, decimal? priceFrom)
        {
            return query.Where(x => (x.NettoPrice * (1 + (decimal)x.Tax / 100)) >= priceFrom);
        }

        public static IQueryable<BookItem> WherePriceTo(this IQueryable<BookItem> query, decimal? priceTo)
        {
            return query.Where(x => (x.NettoPrice * (1 + (decimal)x.Tax / 100)) <= priceTo);
        }

        public static IQueryable<BookItem> WhereIsOnSale(this IQueryable<BookItem> query, bool? isOnSale)
        {
            return isOnSale.HasValue && isOnSale.Value
                ? query.Where(x => x.BookDiscounts.Any(y => y.BookItemID == x.Id))
                : query;
        }

        public static IQueryable<BookItem> OrderBy(this IQueryable<BookItem> query, string sortBy, string sortOrder)
        {
            return sortBy.ToLower() switch
            {
                "popular" => sortOrder.ToLower() == "asc" ? query.OrderBy(x => x.SoldUnits) : query.OrderByDescending(x => x.SoldUnits),
                "price" => sortOrder.ToLower() == "asc" ? query.OrderBy(x => x.NettoPrice * (1 + (decimal)x.Tax / 100)) : query.OrderByDescending(x => x.NettoPrice * (1 + (decimal)x.Tax / 100)),
                "alphabetical" => sortOrder.ToLower() == "asc" ? query.OrderBy(x => x.Book.Title) : query.OrderByDescending(x => x.Book.Title),
                "recentlyadded" => sortOrder.ToLower() == "asc" ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                "score" => sortOrder.ToLower() == "asc" ? query.OrderBy(x => x.Score) : query.OrderByDescending(x => x.Score),
                _ => query
            };
        }

        public static IQueryable<BookItem> SearchBy(this IQueryable<BookItem> query, string searchPhrase)
        {
            return query.Where(x =>
                x.Book.Title.Contains(searchPhrase) ||
                x.Book.BookAuthors.Any(a => a.Author.Name.Contains(searchPhrase) || a.Author.Surname.Contains(searchPhrase)) ||
                x.Book.BookCategories.Any(c => c.Category.Name.Contains(searchPhrase)));
        }

        public static IQueryable<BookItem> WhereNumberOfElements(this IQueryable<BookItem> query, int? numberOfElements)
        {
            return query.Take((int)numberOfElements);
        }
        #endregion
        #region OrderFilters
        public static IQueryable<Order> ApplyOrderFilters(this IQueryable<Order> query, OrderFiltersDto filters)
        {
            if (filters == null)
                return query;

            if (filters.OrderStatusId != null)
                query = query.WhereOrderStatus(filters.OrderStatusId);

            return query;
        }

        public static IQueryable<Order> WhereOrderStatus(this IQueryable<Order> query, int? orderStatusId)
        {
            return query.Where(x => x.OrderStatusID == orderStatusId);
        }
        #endregion
    }

}
