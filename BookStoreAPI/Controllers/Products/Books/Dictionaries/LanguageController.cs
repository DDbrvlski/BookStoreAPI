using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.Books.BookDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.Books.Dictionaries
{
    /// <summary>
    /// Controller for managing languages.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : CRUDController<Language>
    {
        public LanguageController(IBaseService<Language> baseService) : base(baseService)
        {
        }
    }
}
