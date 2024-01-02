using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Rentals.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Rentals.Dictionaries
{
    /// <summary>
    /// Controller for managing rental statuses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RentalStatusController : CRUDController<RentalStatus>
    {
        public RentalStatusController(BookStoreContext context, IBaseService<RentalStatus> baseService, ILogger<RentalStatus> logger) : base(context, baseService, logger)
        {
        }
    }
}
