using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Supplies.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Supplies;
using BookStoreViewModels.ViewModels.Supply;

namespace BookStoreAPI.Controllers.Supplies.Dictionaries
{
    /// <summary>
    /// Controller for managing suppliers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController(ISupplierService supplierService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<SupplierShortViewModel>> GetAllSuppliersAsync()
        {
            var suppliers = await supplierService.GetSuppliersShortDataAsync();
            return Ok(suppliers);
        }

        [HttpGet("{supplierId}")]
        public async Task<ActionResult<SupplierViewModel>> GetSupplierDetailsAsync(int supplierId)
        {
            var supplier = await supplierService.GetSuppliersDataAsync(supplierId);
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSupplierAsync(SupplierPostViewModel supplierData)
        {
            await supplierService.AddNewSupplierAsync(supplierData);
            return NoContent();
        }

        [HttpPut("{supplierId}")]
        public async Task<IActionResult> UpdateSupplierAsync(int supplierId, SupplierPostViewModel supplierData)
        {
            await supplierService.UpdateSupplierAsync(supplierId, supplierData);
            return NoContent();
        }

        [HttpDelete("{supplierId}")]
        public async Task<IActionResult> DeactivateSupplierAsync(int supplierId)
        {
            await supplierService.DeactivateSupplierAsync(supplierId);
            return NoContent();
        }
    }
}
