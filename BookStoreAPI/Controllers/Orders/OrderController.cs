﻿using BookStoreAPI.Services.Invoices;
using BookStoreAPI.Services.Orders;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Orders;
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
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetAllOrdersAsync()
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize("OrderRead")]
        public async Task<ActionResult<OrderDetailsViewModel>> GetOrderByIdAsync(int id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpGet]
        [Route("Invoice")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GenerateInvoice(int orderId)
        {
            var document = await invoiceService.CreateInvoice(orderId);

            byte[] pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }
    }
}
