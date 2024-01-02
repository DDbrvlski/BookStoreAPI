using BookStoreAPI.Helpers;
using BookStoreAPI.Helpers.BaseController;
using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Services.Notifications;
using BookStoreData.Data;
using BookStoreData.Models.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController(IContactService contactService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact contact)
        {
            await contactService.CreateContactMessage(contact);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
        {
            var contacts = await contactService.GetAllContacts();
            return Ok(contacts);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AnswerToContact(int id, string content)
        {
            await contactService.AnswerToContact(id, content);
            return NoContent();
        }
    }
}
