using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Transactions.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Transactions.Dictionaries
{
    /// <summary>
    /// Controller for managing transaction status.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsStatusController : CRUDController<TransactionsStatus>
    {
        public TransactionsStatusController(BookStoreContext context, IBaseService<TransactionsStatus> baseService, ILogger<TransactionsStatus> logger) : base(context, baseService, logger)
        {
        }
    }
}
