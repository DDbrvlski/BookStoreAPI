using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Customers.AddressDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

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
