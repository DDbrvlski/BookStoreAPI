using BookStoreData.Data;
using BookStoreAPI.Helpers.BaseController;
using BookStoreData.Models.Supplies.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Supplies;
using BookStoreViewModels.ViewModels.Supply;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize("SupplierRead")]
        public async Task<ActionResult<SupplierShortViewModel>> GetAllSuppliersAsync()
        {
            var suppliers = await supplierService.GetSuppliersShortDataAsync();
            return Ok(suppliers);
        }

        [HttpGet("{supplierId}")]
        [Authorize("SupplierRead")]
        public async Task<ActionResult<SupplierViewModel>> GetSupplierDetailsAsync(int supplierId)
        {
            var supplier = await supplierService.GetSuppliersDataAsync(supplierId);
            return Ok(supplier);
        }

        [HttpPost]
        [Authorize("SupplierWrite")]
        public async Task<IActionResult> AddNewSupplierAsync(SupplierPostViewModel supplierData)
        {
            await supplierService.AddNewSupplierAsync(supplierData);
            return NoContent();
        }

        [HttpPut("{supplierId}")]
        [Authorize("SupplierEdit")]
        public async Task<IActionResult> UpdateSupplierAsync(int supplierId, SupplierPostViewModel supplierData)
        {
            await supplierService.UpdateSupplierAsync(supplierId, supplierData);
            return NoContent();
        }

        [HttpDelete("{supplierId}")]
        [Authorize("SupplierDelete")]
        public async Task<IActionResult> DeactivateSupplierAsync(int supplierId)
        {
            await supplierService.DeactivateSupplierAsync(supplierId);
            return NoContent();
        }
    }
}
