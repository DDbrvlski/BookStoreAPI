using BookStoreAPI.Services.Orders;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Employee}, {UserRoles.Admin}")]
    public class OrderController(IOrderService orderService) : ControllerBase
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
    }
}
