using BookStoreAPI.Services.Supplies;
using BookStoreViewModels.ViewModels.Supply;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Supplies
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController(ISupplyService supplyService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplyViewModel>>> GetAllSuppliesAsync()
        {
            var supplies = await supplyService.GetAllSuppliesAsync();
            return Ok(supplies);
        }

        [HttpGet("{supplyId}")]
        public async Task<ActionResult<SupplyDetailsViewModel>> GetSupplyDetailsAsync(int supplyId)
        {
            var supply = await supplyService.GetSupplyAsync(supplyId);
            return Ok(supply);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSupplyAsync(SupplyPostViewModel supplyData)
        {
            await supplyService.AddNewSupplyAsync(supplyData);
            return NoContent();
        }

        [HttpPut("{supplyId}")]
        public async Task<IActionResult> UpdateSupplyAsync(int supplyId, SupplyPutViewModel supplyData)
        {
            await supplyService.UpdateSupplyAsync(supplyId, supplyData);
            return NoContent();
        }

        [HttpDelete("{supplyId}")]
        public async Task<IActionResult> DeactivateSupplyAsync(int supplyId)
        {
            await supplyService.DeactivateSupplyAsync(supplyId);
            return NoContent();
        }
    }
}
