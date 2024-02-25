using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.Books.BookDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

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
