using BookStoreAPI.Services.Notifications;
using BookStoreData.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController(INewsletterService newsletterService) : ControllerBase
    {
        [HttpGet]
        [Authorize("NewsletterRead")]
        public async Task<ActionResult<IEnumerable<Newsletter>>> GetAllNewsletters()
        {
            var newsletters = await newsletterService.GetAllNewslettersAsync();
            return Ok(newsletters);
        }

        [HttpPut("{id}")]
        [Authorize("NewsletterEdit")]
        public async Task<IActionResult> EditNewsletter(int id, Newsletter newNewsletter)
        {
            await newsletterService.EditNewsletterAsync(id, newNewsletter);
            return NoContent();
        }

        [HttpPost]
        [Route("Subscriber")]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewNewsletterUser(string email)
        {
            await newsletterService.AddToNewsletterSubscribersAsync(email);
            return Ok("Zapisano do newslettera.");
        }

        [HttpPost]
        [Authorize("NewsletterWrite")]
        public async Task<IActionResult> CreateNewsletter(Newsletter newsletter)
        {
            await newsletterService.CreateNewsletterAsync(newsletter);
            return Ok();
        }

        //TESTOWY ENDPOINT DO WYSYŁANIA NEWSLETTERA
        [HttpGet]
        [Route("SEND")]
        public async Task SendNewsletters()
        {
            await newsletterService.SendNewsletterToSubscribersAsync();
        }
    }
}
