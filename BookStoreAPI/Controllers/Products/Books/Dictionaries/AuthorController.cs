using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.Books.BookDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.Books.Dictionaries
{
    /// <summary>
    /// Controller for managing authors.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : CRUDController<Author>
    {
        public AuthorController(BookStoreContext context, IBaseService<Author> baseService, ILogger<Author> logger) : base(context, baseService, logger)
        {
        }
    }
}
