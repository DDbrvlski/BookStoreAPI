using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Transactions.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Transactions.Dictionaries
{
    /// <summary>
    /// Controller for managing payment methods.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : CRUDController<PaymentMethod>
    {
        public PaymentMethodController(IBaseService<PaymentMethod> baseService) : base(baseService)
        {
        }
    }
}
