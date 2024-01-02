using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.PageContent;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooterColumnsController : CRUDController<FooterColumns>
    {
        public FooterColumnsController(BookStoreContext context, IBaseService<FooterColumns> baseService, ILogger<FooterColumns> logger) : base(context, baseService, logger)
        {
        }
    }
}
