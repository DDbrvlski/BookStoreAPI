using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Media;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Media
{
    /// <summary>
    /// Controller for managing images.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : CRUDController<Images>
    {
        public ImagesController
            (BookStoreContext context, 
            IBaseService<Images> baseService, 
            ILogger<Images> logger)
            : base(context, baseService, logger)
        {
        }
    }
}
