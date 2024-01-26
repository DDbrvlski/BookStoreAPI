using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Stock;
using BookStoreData.Data;
using BookStoreData.Models.Supplies;
using BookStoreViewModels.ViewModels.Stock;
using BookStoreViewModels.ViewModels.Supply;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;

namespace BookStoreAPI.Services.Supplies
{
    public interface ISupplyGoodsService
    {
        Task AddNewSupplyBooksAsync(int supplyId, List<SupplyBookPostViewModel> supplyBooks);
        Task DeactivateAllSupplyGoodsAsync(int supplyId);
        Task DeactivateSupplyGoodsAsync(int supplyId, List<int>? supplyGoodsToDeactivate);
        Task UpdateSupplyBooksAsync(int supplyId, List<SupplyBookPostViewModel> supplyBooks);
    }

    public class SupplyGoodsService(BookStoreContext context, IStockAmountService stockAmountService) : ISupplyGoodsService
    {
        public async Task AddNewSupplyBooksAsync(int supplyId, List<SupplyBookPostViewModel> supplyBooks)
        {
            if (supplyBooks.Any())
            {
                List<BookItemStockAmountUpdateViewModel> bookItems = new();
                List<SupplyGoods> supplyGoods = new List<SupplyGoods>();

                foreach (var supplyBook in supplyBooks)
                {
                    SupplyGoods supplyGood = new SupplyGoods();
                    supplyGood.SupplyID = supplyId;
                    supplyGood.BookItemID = supplyBook.BookItemId;
                    supplyGood.Quantity = supplyBook.Quantity ?? 1;
                    supplyGood.BruttoPrice = supplyBook.BruttoPrice;
                    bookItems.Add(new BookItemStockAmountUpdateViewModel() { BookItemId = (int)supplyGood.BookItemID, Quantity = supplyGood.Quantity });

                    supplyGoods.Add(supplyGood);
                }

                await context.SupplyGoods.AddRangeAsync(supplyGoods);

                await DatabaseOperationHandler.TryToSaveChangesAsync(context);

                await stockAmountService.UpdateStockAmountAsync(bookItems);
            }
        }
        
        public async Task UpdateSupplyBooksAsync(int supplyId, List<SupplyBookPostViewModel> supplyBooks)
        {
            if (supplyBooks.Any())
            {
                var existingSupplyGoodsIds = await context.SupplyGoods
                    .Where(x => x.SupplyID == supplyId && x.IsActive)
                    .Select(x => x.BookItemID)
                    .ToListAsync();

                List<int> newSupplyGoodsIds = supplyBooks.Select(x => x.BookItemId).ToList();

                var supplyGoodsToDeactivate = existingSupplyGoodsIds.Except(newSupplyGoodsIds).ToList();
                var supplyGoodsToAdd = supplyBooks.Where(x => !existingSupplyGoodsIds.Contains(x.BookItemId)).ToList();

                if (supplyGoodsToDeactivate.Any())
                {
                    await DeactivateSupplyGoodsAsync(supplyId, supplyGoodsToDeactivate);
                }

                if (supplyGoodsToAdd.Any())
                {
                    await AddNewSupplyBooksAsync(supplyId, supplyGoodsToAdd);
                }
            }
        }
        public async Task DeactivateSupplyGoodsAsync(int supplyId, List<int>? supplyGoodsToDeactivate)
        {
            List<SupplyGoods>? supplyGoods = new();
            if (supplyGoodsToDeactivate.IsNullOrEmpty())
            {
                supplyGoods = await context.SupplyGoods
                    .Where(x => x.IsActive && x.Id == supplyId)
                    .ToListAsync();
            }
            else
            {
                supplyGoods = await context.SupplyGoods
                    .Where(x => x.IsActive && x.SupplyID == supplyId && supplyGoodsToDeactivate.Contains((int)x.BookItemID))
                    .ToListAsync();
            }
            
            List<BookItemStockAmountUpdateViewModel> bookItems = new();

            foreach (var supplyGood in supplyGoods)
            {
                supplyGood.IsActive = false;
                bookItems.Add(new BookItemStockAmountUpdateViewModel() { BookItemId = (int)supplyGood.BookItemID, Quantity = -supplyGood.Quantity });
            }

            await stockAmountService.UpdateStockAmountAsync(bookItems);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateAllSupplyGoodsAsync(int supplyId)
        {
            await DeactivateSupplyGoodsAsync(supplyId, new List<int>());
        }
    }
}
