using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Orders.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;
using BookStoreData.Models.Transactions.Dictionaries;

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
