using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Delivery.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Delivery.Dictionaries
{
    /// <summary>
    /// Controller for managing shipping statuses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingStatusController : CRUDController<ShippingStatus>
    {
        public ShippingStatusController(BookStoreContext context, IBaseService<ShippingStatus> baseService, ILogger<ShippingStatus> logger) : base(context, baseService, logger)
        {
        }
    }
}
