using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;
using BookStoreData.Models.Transactions.Dictionaries;

namespace BookStoreAPI.Controllers.Products.BookItems.Dictionaries
{
    /// <summary>
    /// Controller for managing file formats.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileFormatController : CRUDController<FileFormat>
    {
        public FileFormatController(IBaseService<FileFormat> baseService) : base(baseService)
        {
        }
    }
}
