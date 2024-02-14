using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Data;
using BookStoreData.Models.Customers.AddressDictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Customers.Dictionaries.Address
{
    /// <summary>
    /// Controller for managing cities.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : CRUDController<City>
    {
        public CityController
            (BookStoreContext context, 
            IBaseService<City> baseService,
            ILogger<City> logger)
            : base(context, baseService, logger)
        {
        }
    }
}
