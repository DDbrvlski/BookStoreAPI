using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.PageContent;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;
using BookStoreData.Models.Transactions.Dictionaries;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooterColumnsController : CRUDController<FooterColumns>
    {
        public FooterColumnsController(IBaseService<FooterColumns> baseService) : base(baseService)
        {
        }
    }
}
