using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.Orders.Dictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Orders.Dictionaries
{
    /// <summary>
    /// Controller for managing delivery methods.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMethodController : CRUDController<DeliveryMethod>
    {
        public DeliveryMethodController(IBaseService<DeliveryMethod> baseService) : base(baseService)
        {
        }
    }
}
