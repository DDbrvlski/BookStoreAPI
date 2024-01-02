using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Supplies.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Supplies.Dictionaries
{
    /// <summary>
    /// Controller for managing suppliers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : CRUDController<Supplier>
    {
        public SupplierController(BookStoreContext context, IBaseService<Supplier> baseService, ILogger<Supplier> logger) : base(context, baseService, logger)
        {
        }
    }
}
