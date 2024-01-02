using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.BookItems.Dictionaries
{
    /// <summary>
    /// Controller for managing availabilites.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : CRUDController<Availability>
    {
        public AvailabilityController(BookStoreContext context, IBaseService<Availability> baseService, ILogger<Availability> logger) : base(context, baseService, logger)
        {
        }
    }
}
