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
    /// Controller for managing order statuses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : CRUDController<OrderStatus>
    {
        public OrderStatusController(IBaseService<OrderStatus> baseService) : base(baseService)
        {
        }
    }
}
