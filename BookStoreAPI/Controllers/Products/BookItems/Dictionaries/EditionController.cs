using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems.Dictionaries
{
    /// <summary>
    /// Controller for managing editions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EditionController : CRUDController<Edition>
    {
        public EditionController(IBaseService<Edition> baseService) : base(baseService)
        {
        }
    }
}
