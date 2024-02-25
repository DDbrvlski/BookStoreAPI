using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Helpers.BaseController
{
    public class BaseController
        <TEntity>
        (IBaseService<TEntity> baseService) 
        : ControllerBase 
        where TEntity : BaseEntity
    {
        protected virtual async Task<ActionResult<IEnumerable<TEntity>>> GetEntitiesAsync()
        {
            var entities = await baseService.GetEntitiesAsync();
            return Ok(entities);
        }
        protected virtual async Task<ActionResult<TEntity>> GetEntityByIdAsync(int id)
        {
            var entity = await baseService.GetEntityByIdAsync(id);
            return Ok(entity);
        }
        protected virtual async Task<ActionResult> AddNewEntityAsync(TEntity entity)
        {
            await baseService.AddNewEntityAsync(entity);
            return Created();
        }
        protected virtual async Task<ActionResult> UpdateEntityAsync(int id, TEntity entity)
        {
            await baseService.UpdateEntityAsync(id, entity);
            return NoContent();
        }
        protected virtual async Task<ActionResult> DeactivateEntityAsync(int id)
        {
            await baseService.DeactivateEntityAsync(id);
            return NoContent();
        }
    }
}

