﻿using BookStoreAPI.Helpers;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Availability;
using BookStoreData.Data;
using BookStoreData.Models.Products.BookItems;
using BookStoreDto.Dtos.Availability;
using BookStoreDto.Dtos.Products.StockAmount;
using BookStoreDto.Dtos.Stock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services.Stock
{
    public interface IStockAmountService
    {
        Task<ActionResult<IEnumerable<StockAmountDto>>> GetAllStockAmountAsync();
        Task<ActionResult<StockAmountDto?>> GetStockAmountByIdAsync(int id);
        Task<int> GetStockAmountForBookItemByIdAsync(int bookItemId);

        Task CreateStockAmountAsync(int bookItemId, int? amount);
        Task UpdateStockAmountAsync(int bookItemId, int amount);
        Task UpdateStockAmountAsync(List<BookItemStockAmountUpdateDto> bookItems);
        Task DeactivateStockAmountAsync(int bookItemId);
    }

    public class StockAmountService
        (BookStoreContext context, 
        IAvailabilityService availabilityService)
        : IStockAmountService
    {
        public async Task<ActionResult<IEnumerable<StockAmountDto>>> GetAllStockAmountAsync()
        {
            return await context.StockAmount
                .Where(x => x.IsActive == true)
                .Select(x => new StockAmountDto
                {
                    Id = x.Id,
                    BookTitle = x.BookItem.Book.Title,
                    Amount = x.Amount,
                    BookItemID = x.BookItemID
                })
                .ToListAsync();
        }
        public async Task<ActionResult<StockAmountDto?>> GetStockAmountByIdAsync(int id)
        {
            return await context.StockAmount
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new StockAmountDto()
                {
                    Id = x.Id,
                    BookTitle = x.BookItem.Book.Title,
                    Amount = x.Amount,
                    BookItemID = x.BookItemID
                }).FirstAsync();
        }
        public async Task CreateStockAmountAsync(int bookItemId, int? amount)
        {
            if (amount == null)
            {
                amount = 0;
            }

            StockAmount stockAmount = new()
            {
                BookItemID = bookItemId,
                Amount = (int)amount
            };

            await context.StockAmount.AddAsync(stockAmount);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await availabilityService.UpdateBookItemAvailabilityAsync(bookItemId, stockAmount.Amount);
        }
        public async Task DeactivateStockAmountAsync(int bookItemId)
        {
            var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

            if (stockAmount == null)
            {
                throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
            }

            await availabilityService.DisableAvailabilityForBookItemAsync(bookItemId);
            stockAmount.IsActive = false;
            stockAmount.ModifiedDate = DateTime.UtcNow;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task UpdateStockAmountAsync(int bookItemId, int amount)
        {
            var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

            if (stockAmount == null)
            {
                throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
            }

            stockAmount.Amount = amount;
            stockAmount.ModifiedDate = DateTime.UtcNow;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await availabilityService.UpdateBookItemAvailabilityAsync(bookItemId, stockAmount.Amount);
        }
        public async Task UpdateStockAmountAsync(List<BookItemStockAmountUpdateDto> bookItems)
        {
            var bookItemIds = bookItems.Select(y => y.BookItemId).ToList();

            var bookItemsToUpdate = await context.StockAmount
                .Where(x => x.IsActive && bookItemIds.Contains((int)x.BookItemID))
                .ToListAsync();

            foreach (var bookItem in bookItemsToUpdate)
            {
                var updateData = bookItems.FirstOrDefault(y => y.BookItemId == bookItem.BookItemID);

                if (updateData != null)
                {
                    bookItem.Amount += updateData.Quantity;
                    if (bookItem.Amount < 0)
                    {
                        throw new BadRequestException($"Brak wystarczającej ilości produktu na magazynie.");
                    }
                }
            }

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            List<AvailabilityBookItemsCheckDto> stockBookItems = new();
            foreach (var bookItem in bookItemsToUpdate)
            {
                stockBookItems.Add(new AvailabilityBookItemsCheckDto() { BookItemId = (int)bookItem.BookItemID, StockAmount = bookItem.Amount });
            }

            await availabilityService.UpdateBookItemsAvailabilityAsync(stockBookItems);
        }

        public async Task<int> GetStockAmountForBookItemByIdAsync(int bookItemId)
        {
            var stockAmount = await context.StockAmount.FirstOrDefaultAsync(x => x.IsActive && x.BookItemID == bookItemId);

            if (stockAmount == null)
            {
                throw new NotFoundException("Nie znaleziono obiektu stock amount dla produktu.");
            }

            return stockAmount.Amount;
        }

    }
}
