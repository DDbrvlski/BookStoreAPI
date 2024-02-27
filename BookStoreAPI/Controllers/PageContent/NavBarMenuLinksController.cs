using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.PageContent;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavBarMenuLinksController : CRUDController<NavBarMenuLinks>
    {
        public NavBarMenuLinksController(IBaseService<NavBarMenuLinks> baseService) : base(baseService)
        {
        }
    }
}
