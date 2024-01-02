using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Data;
using BookStoreData.Models.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Helpers.BaseController
{
    public class CRUDController<TEntity> : BaseController<TEntity> where TEntity : BaseEntity
    {
        public CRUDController
            (BookStoreContext context, 
            IBaseService<TEntity> baseService, 
            
            ILogger<TEntity> logger)
            : base(context, baseService, logger)
        {
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteEntity(int id)
        {
            return await DeactivateEntityAsync(id);
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetEntities()
        {
            return await GetEntitiesAsync();
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> GetEntity(int id)
        {
            return await GetEntityByIdAsync(id);
        }

        [HttpPost]
        public virtual async Task<ActionResult> PostEntity(TEntity entity)
        {
            return await AddNewEntityAsync(entity);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> PutEntity(int id, [FromBody] TEntity entity)
        {
            return await UpdateEntityAsync(id, entity);
        }

    }

}
