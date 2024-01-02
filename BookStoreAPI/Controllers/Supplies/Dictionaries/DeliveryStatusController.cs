using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Supplies.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Supplies.Dictionaries
{
    /// <summary>
    /// Controller for managing delivery statuses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryStatusController : CRUDController<DeliveryStatus>
    {
        public DeliveryStatusController(BookStoreContext context, IBaseService<DeliveryStatus> baseService, ILogger<DeliveryStatus> logger) : base(context, baseService, logger)
        {
        }
    }
}
