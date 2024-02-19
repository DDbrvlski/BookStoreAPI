using BookStoreAPI.Services.Orders;
using BookStoreAPI.Services.Users;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Accounts.User;
using BookStoreDto.Dtos.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreAPI.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController
        (IUserService userService,
        IOrderService orderService)
        : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<UserDataDto>> GetUserData()
        {
            var user = await userService.GetUserDataAsync();
            return Ok(user);
        }

        [HttpGet("Address")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<IEnumerable<UserAddressDto>>> GetUserDataAddress()
        {
            var userAddress = await userService.GetUserAddressDataAsync();
            return Ok(userAddress);
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> DeactivateUserAccount()
        {
            await userService.DeactivateUserAccountAsync();
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> EditUserData(UserDataDto userData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await userService.EditUserDataAsync(userData);
            return NoContent();
        }

        [HttpPut("Password")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> EditUserPassword(UserChangePasswordDto userData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await userService.EditUserPasswordAsync(userData);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("Address")]
        public async Task<IActionResult> EditUserAddressData(UserAddressDto userData)
        {
            await userService.EditUserAddressDataAsync(userData);
            return NoContent();
        }

        [HttpPost]
        [Route("Order")]
        public async Task<IActionResult> CreateNewOrder(OrderPostDto orderModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await orderService.CreateNewOrderAsync(orderModel);
            return Created();
        }

        [HttpGet]
        [Route("Order")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrdersAsync([FromQuery] OrderFiltersDto orderFilters)
        {
            var orders = await orderService.GetUserOrdersAsync(orderFilters);
            return Ok(orders);
        }

        [HttpGet]
        [Route("Order/{id}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<OrderDetailsDto>> GetUserOrderByIdAsync(int id)
        {
            var order = await orderService.GetUserOrderByIdAsync(id);
            return Ok(order);
        }


        [HttpPost]
        [Route("Order/DiscountCode")]
        public async Task<ActionResult<OrderDiscountCheckDto>> ApplyDiscountForOrder(OrderDiscountCheckDto orderDiscountModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var orderDiscount = await orderService.ApplyDiscountCodeToOrderAsync(orderDiscountModel);
            return Ok(orderDiscount);
        }
    }
}
