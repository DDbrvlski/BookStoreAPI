using BookStoreAPI.Services.Invoices;
using BookStoreAPI.Services.Orders;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace BookStoreAPI.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService, IInvoiceService invoiceService) : ControllerBase
    {
        [HttpGet]
        [Authorize("OrderRead")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize("OrderRead")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderByIdAsync(int id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpGet]
        [Route("Invoice/Template")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GenerateInvoice(int orderId)
        {
            var document = await invoiceService.CreateInvoice(orderId);

            byte[] pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }

        [HttpGet]
        [Route("Invoice")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GenerateInvoiceByTemplate(int orderId)
        {
            byte[] pdfBytes = await invoiceService.CreateInvoiceByDocxTemplate(orderId);

            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }

    }
}
