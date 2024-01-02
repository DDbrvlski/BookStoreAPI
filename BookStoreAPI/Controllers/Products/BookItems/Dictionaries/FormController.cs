using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.BookItems.Dictionaries
{
    /// <summary>
    /// Controller for managing forms.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : CRUDController<Form>
    {
        public FormController(BookStoreContext context, IBaseService<Form> baseService, ILogger<Form> logger) : base(context, baseService, logger)
        {
        }
    }
}
