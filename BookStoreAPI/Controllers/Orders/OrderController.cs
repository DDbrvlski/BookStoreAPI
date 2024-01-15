using BookStoreAPI.Services.Invoices;
using BookStoreAPI.Services.Orders;
using BookStoreViewModels.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace BookStoreAPI.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = $"{UserRoles.Employee}, {UserRoles.Admin}")]
    public class OrderController(IOrderService orderService, IInvoiceService invoiceService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetAllOrdersAsync()
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsViewModel>> GetOrderByIdAsync(int id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpGet]
        [Route("Invoice")]
        public async Task<IActionResult> GenerateInvoice(int orderId)
        {
            var document = await invoiceService.CreateInvoice(orderId);

            // Generate PDF content as byte array
            byte[] pdfBytes = document.GeneratePdf();

            // Return PDF file as response
            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }


    }
}
