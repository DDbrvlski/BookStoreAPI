using BookStoreAPI.Services.Supplies;
using BookStoreDto.Dtos.Supply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Supplies
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController(ISupplyService supplyService) : ControllerBase
    {
        [HttpGet]
        [Authorize("SupplyRead")]
        public async Task<ActionResult<IEnumerable<SupplyDto>>> GetAllSuppliesAsync()
        {
            var supplies = await supplyService.GetAllSuppliesAsync();
            return Ok(supplies);
        }

        [HttpGet("{supplyId}")]
        [Authorize("SupplyRead")]
        public async Task<ActionResult<SupplyDetailsDto>> GetSupplyDetailsAsync(int supplyId)
        {
            var supply = await supplyService.GetSupplyAsync(supplyId);
            return Ok(supply);
        }

        [HttpPost]
        [Authorize("SupplyWrite")]
        public async Task<IActionResult> AddNewSupplyAsync(SupplyPostDto supplyData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await supplyService.AddNewSupplyAsync(supplyData);
            return NoContent();
        }

        [HttpPut("{supplyId}")]
        [Authorize("SupplyEdit")]
        public async Task<IActionResult> UpdateSupplyAsync(int supplyId, SupplyPutDto supplyData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await supplyService.UpdateSupplyAsync(supplyId, supplyData);
            return NoContent();
        }

        [HttpDelete("{supplyId}")]
        [Authorize("SupplyDelete")]
        public async Task<IActionResult> DeactivateSupplyAsync(int supplyId)
        {
            await supplyService.DeactivateSupplyAsync(supplyId);
            return NoContent();
        }
    }
}
