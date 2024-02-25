using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreData.Data;
using BookStoreData.Models.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Helpers.BaseService
{
    public interface IBaseService
        <TEntity>
    {
        Task<TEntity?> GetEntityByIdAsync(int id);
        Task<IEnumerable<TEntity>?> GetEntitiesAsync();
        Task AddNewEntityAsync(TEntity entity);
        Task UpdateEntityAsync(int id, TEntity entity);
        Task DeactivateEntityAsync(int id);
    }
    public class BaseService
        <TEntity>
        (BookStoreContext context)
        : IBaseService<TEntity>
        where TEntity : BaseEntity
    {
        public virtual async Task<TEntity?> GetEntityByIdAsync(int id)
        {
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.IsActive && x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Nie znaleziono");
            }

            return entity;
        }
        public virtual async Task<IEnumerable<TEntity>?> GetEntitiesAsync()
        {
            return await context.Set<TEntity>().Where(x => x.IsActive).ToListAsync();
        }
        public virtual async Task AddNewEntityAsync(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public virtual async Task UpdateEntityAsync(int id, TEntity entity)
        {
            var entityById = await GetEntityByIdAsync(id);
            entityById.CopyProperties(entity);
            entityById.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public virtual async Task DeactivateEntityAsync(int id)
        {
            var entity = await GetEntityByIdAsync(id);
            
            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
