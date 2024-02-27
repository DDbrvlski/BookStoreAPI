using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.Transactions.Dictionaries;
using Microsoft.AspNetCore.Mvc;

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
