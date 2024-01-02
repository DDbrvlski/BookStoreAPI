using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Orders.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Orders.Dictionaries
{
    /// <summary>
    /// Controller for managing delivery methods.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMethodController : CRUDController<DeliveryMethod>
    {
        public DeliveryMethodController(BookStoreContext context, IBaseService<DeliveryMethod> baseService, ILogger<DeliveryMethod> logger) : base(context, baseService, logger)
        {
        }
    }
}
