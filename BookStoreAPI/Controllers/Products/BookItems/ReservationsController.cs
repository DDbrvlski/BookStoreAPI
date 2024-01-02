using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.BookItems;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : CRUDController<Reservations>
    {
        public ReservationsController(BookStoreContext context, IBaseService<Reservations> baseService, ILogger<Reservations> logger) : base(context, baseService, logger)
        {
        }
    }
}
