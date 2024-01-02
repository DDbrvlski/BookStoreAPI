using BookStoreAPI.Helpers;
using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Data;
using BookStoreData.Models.PageContent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.PageContent
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavBarMenuLinksController : CRUDController<NavBarMenuLinks>
    {
        public NavBarMenuLinksController(BookStoreContext context, IBaseService<NavBarMenuLinks> baseService, ILogger<NavBarMenuLinks> logger) : base(context, baseService, logger)
        {
        }
    }
}
