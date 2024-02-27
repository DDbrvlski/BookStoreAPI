using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems.Dictionaries
{
    /// <summary>
    /// Controller for managing availabilites.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : CRUDController<Availability>
    {
        public AvailabilityController(IBaseService<Availability> baseService) : base(baseService)
        {
        }
    }
}
