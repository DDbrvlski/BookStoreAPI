using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreData.Models.Products.Books.BookDictionaries;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.Books.Dictionaries
{
    /// <summary>
    /// Controller for managing publishers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : CRUDController<Publisher>
    {
        public PublisherController(IBaseService<Publisher> baseService) : base(baseService)
        {
        }
    }
}
