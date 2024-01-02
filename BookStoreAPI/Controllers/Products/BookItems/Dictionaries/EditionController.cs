using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.BookItems.Dictionaries
{
    /// <summary>
    /// Controller for managing editions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EditionController : CRUDController<Edition>
    {
        public EditionController(BookStoreContext context, IBaseService<Edition> baseService, ILogger<Edition> logger) : base(context, baseService, logger)
        {
        }
    }
}
