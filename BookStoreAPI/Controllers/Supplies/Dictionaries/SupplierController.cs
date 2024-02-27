using BookStoreAPI.Services.Supplies;
using BookStoreDto.Dtos.Supply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<SupplierShortDto>> GetAllSuppliersAsync()
        {
            var suppliers = await supplierService.GetSuppliersShortDataAsync();
            return Ok(suppliers);
        }

        [HttpGet("{supplierId}")]
        [Authorize("SupplierRead")]
        public async Task<ActionResult<SupplierDto>> GetSupplierDetailsAsync(int supplierId)
        {
            var supplier = await supplierService.GetSuppliersDataAsync(supplierId);
            return Ok(supplier);
        }

        [HttpPost]
        [Authorize("SupplierWrite")]
        public async Task<IActionResult> AddNewSupplierAsync(SupplierPostDto supplierData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await supplierService.AddNewSupplierAsync(supplierData);
            return NoContent();
        }

        [HttpPut("{supplierId}")]
        [Authorize("SupplierEdit")]
        public async Task<IActionResult> UpdateSupplierAsync(int supplierId, SupplierPostDto supplierData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
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
