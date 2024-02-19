using BookStoreAPI.Services.Rentals;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Rentals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Rentals
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController(BookStoreContext context, IRentalService rentalService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> PostRentalAsync(RentalPostDto rentalModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await rentalService.CreateRentalAsync(rentalModel);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<IEnumerable<RentalDto>>> GetUserRentalsAsync(int rentalStatusId = 0)
        {
            var userRentals = await rentalService.GetUserRentalsByRentalStatusIdAsync(rentalStatusId);
            return Ok(userRentals);
        }
    }
}
