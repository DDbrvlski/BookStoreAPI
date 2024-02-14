using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Data;
using BookStoreData.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreAPI.Helpers.BaseController
{
    public class CRUDController<TEntity> : BaseController<TEntity> where TEntity : BaseEntity
    {
        private readonly string claimType = typeof(TEntity).Name;
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
            var isAuthorized = await CheckAuthorization("Delete");
            if (!isAuthorized)
            {
                return Forbid();
            }

            return await DeactivateEntityAsync(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetEntities()
        {            
            return await GetEntitiesAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public virtual async Task<ActionResult<TEntity>> GetEntity(int id)
        {
            return await GetEntityByIdAsync(id);
        }

        [HttpPost]
        public virtual async Task<ActionResult> PostEntity(TEntity entity)
        {
            var isAuthorized = await CheckAuthorization("Write");
            if (!isAuthorized)
            {
                return Forbid();
            }

            return await AddNewEntityAsync(entity);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> PutEntity(int id, [FromBody] TEntity entity)
        {
            var isAuthorized = await CheckAuthorization("Edit");
            if (!isAuthorized)
            {
                return Forbid();
            }

            return await UpdateEntityAsync(id, entity);
        }

        private async Task<bool> CheckAuthorization(string actionName)
        {
            var authorizationService = HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

            var authorizationResult = await authorizationService.AuthorizeAsync(User, claimType + actionName);

            return authorizationResult.Succeeded;
        }
    }

}
