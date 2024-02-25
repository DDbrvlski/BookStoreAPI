using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Data;
using BookStoreData.Models.Customers.AddressDictionaries;
using Microsoft.AspNetCore.Mvc;

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
            (IBaseService<Country> baseService)
            : base(baseService)
        {
        }
    }
}
