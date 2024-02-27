using BookStoreAPI.Services.Payments;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Transactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        //Symulacja płatności
        [HttpPost("{paymentId}")]
        public async Task<IActionResult> MakePaymentAsync(int paymentId)
        {
            await paymentService.MakePaymentAsync(paymentId);
            return NoContent();
        }
    }
}
