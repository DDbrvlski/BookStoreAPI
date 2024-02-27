using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.PageContent;
using Microsoft.AspNetCore.Mvc;

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
