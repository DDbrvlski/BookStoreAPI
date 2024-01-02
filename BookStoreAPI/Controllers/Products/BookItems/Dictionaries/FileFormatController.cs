using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;

namespace BookStoreAPI.Controllers.Products.BookItems.Dictionaries
{
    /// <summary>
    /// Controller for managing file formats.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileFormatController : CRUDController<FileFormat>
    {
        public FileFormatController(BookStoreContext context, IBaseService<FileFormat> baseService, ILogger<FileFormat> logger) : base(context, baseService, logger)
        {
        }
    }
}
