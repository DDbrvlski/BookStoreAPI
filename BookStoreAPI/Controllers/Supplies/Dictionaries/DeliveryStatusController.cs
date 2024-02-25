using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Supplies.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;
using BookStoreData.Models.Transactions.Dictionaries;

namespace BookStoreAPI.Controllers.Supplies.Dictionaries
{
    /// <summary>
    /// Controller for managing delivery statuses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryStatusController : CRUDController<DeliveryStatus>
    {
        public DeliveryStatusController(IBaseService<DeliveryStatus> baseService) : base(baseService)
        {
        }
    }
}
