using BookStoreAPI.Services.Reservation;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Products.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Products.BookItems
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController(IReservationService reservationService) : ControllerBase
    {
        [HttpPost]
        [Route("Store/{id}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> ReservateABookItemAsync(int id)
        {
            await reservationService.ReservateABookItemAsync(id);
            return NoContent();
        }

        [HttpDelete]
        [Route("Store/{id}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> DeactivateReservation(int id)
        {
            await reservationService.DeactivateReservationAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Route("Store")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<IEnumerable<ReservationViewModel>>> GetUserAllReservations()
        {
            var reservations = await reservationService.GetUserAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationsMadeByAllCustomersCMSViewModel>>> GetAllReservationsMadeByCustomersCMSAsync()
        {
            var reservations = await reservationService.GetAllReservationsMadeByCustomersCMSAsync();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationsMadeByCustomerCMSViewModel>> GetAllReservationsMadeBySingleCustomerByIdCMSAsync(int id)
        {
            var reservations = await reservationService.GetAllReservationsMadeBySingleCustomerByIdCMSAsync(id);
            return Ok(reservations);
        }
    }
}
