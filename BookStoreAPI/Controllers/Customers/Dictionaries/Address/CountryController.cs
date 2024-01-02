using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Customers.AddressDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Customers.Dictionaries.Address
{
    /// <summary>
    /// Controller for managing countries.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : CRUDController<Country>
    {
        public CountryController
            (BookStoreContext context, 
            IBaseService<Country> baseService, 
            
            ILogger<Country> logger)
            : base(context, baseService, logger)
        {
        }
    }
}
