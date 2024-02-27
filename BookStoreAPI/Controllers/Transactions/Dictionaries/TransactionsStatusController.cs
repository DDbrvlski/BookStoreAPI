using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.Transactions.Dictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Transactions.Dictionaries
{
    /// <summary>
    /// Controller for managing transaction status.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsStatusController : CRUDController<TransactionsStatus>
    {
        public TransactionsStatusController(IBaseService<TransactionsStatus> baseService) : base(baseService)
        {
        }
    }
}
