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
        public TransactionsStatusController(IBaseService<TransactionsStatus> baseService) : base(baseService)
        {
        }
    }
}
