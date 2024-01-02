using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.Books.BookDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.Books.Dictionaries
{
    /// <summary>
    /// Controller for managing scores.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : CRUDController<Score>
    {
        public ScoreController(BookStoreContext context, IBaseService<Score> baseService, ILogger<Score> logger) : base(context, baseService, logger)
        {
        }
    }
}
