using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.Books.BookDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.Books.Dictionaries
{
    /// <summary>
    /// Controller for managing translators.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TranslatorController : CRUDController<Translator>
    {
        public TranslatorController(BookStoreContext context, IBaseService<Translator> baseService, ILogger<Translator> logger) : base(context, baseService, logger)
        {
        }
    }
}
