using BookStoreAPI.Helpers;
using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Data;
using BookStoreData.Models.Rentals.Dictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Rentals.Dictionaries
{
    /// <summary>
    /// Controller for managing rental types.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RentalTypeController : CRUDController<RentalType>
    {
        public RentalTypeController(BookStoreContext context, IBaseService<RentalType> baseService, ILogger<RentalType> logger) : base(context, baseService, logger)
        {
        }
    }
}
