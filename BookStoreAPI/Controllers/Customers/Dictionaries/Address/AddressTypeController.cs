using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Data;
using BookStoreData.Models.Customers.AddressDictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Customers.Dictionaries.Address
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressTypeController : CRUDController<AddressType>
    {
        public AddressTypeController
            (BookStoreContext context, 
            IBaseService<AddressType> baseService, 
            ILogger<AddressType> logger)
            : base(context, baseService, logger)
        {
        }
    }
}
