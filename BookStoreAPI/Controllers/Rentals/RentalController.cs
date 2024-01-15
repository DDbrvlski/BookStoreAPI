using BookStoreAPI.Services.Rentals;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Rentals;
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
        public async Task<IActionResult> PostRentalAsync(RentalPostViewModel rentalModel)
        {
            await rentalService.CreateRentalAsync(rentalModel);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<IEnumerable<RentalViewModel>>> GetUserRentalsAsync(int rentalStatusId = 0)
        {
            var userRentals = await rentalService.GetUserRentalsByRentalStatusIdAsync(rentalStatusId);
            return Ok(userRentals);
        }
    }
}
