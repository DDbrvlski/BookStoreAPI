using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.Products.Books.BookDictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.Books.Dictionaries
{
    /// <summary>
    /// Controller for managing scores.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : CRUDController<Score>
    {
        public ScoreController(IBaseService<Score> baseService) : base(baseService)
        {
        }
    }
}
