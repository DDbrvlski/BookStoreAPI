using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Helpers
{
    public class DatabaseOperationHandler
    {
        public static async Task HandleDatabaseOperation(Func<Task> databaseOperation, string operationName)
        {
            try
            {
                await databaseOperation.Invoke();
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Błąd podczas operacji {operationName} w bazie danych.", ex);
            }
        }

        public static async Task<IActionResult> TryToSaveChangesAsync(BookStoreContext context)
        {
            try
            {
                await DatabaseOperationHandler.HandleDatabaseOperation(
                    async () => await context.SaveChangesAsync(),
                    "zapisu danych w bazie");
                return new OkResult();
            }
            catch (DatabaseOperationException ex)
            {
                throw new BadRequestException($"Błąd operacji w bazie danych: {ex.InnerException.Message}");
            }
        }

    }
}
