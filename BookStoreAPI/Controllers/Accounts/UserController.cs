using BookStoreAPI.Services.Orders;
using BookStoreAPI.Services.Users;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Accounts.User;
using BookStoreViewModels.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<UserDataViewModel>> GetUserData()
        {
            var user = await userService.GetUserDataAsync();
            return Ok(user);
        }

        [HttpGet("Address")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<IEnumerable<UserAddressViewModel>>> GetUserDataAddress()
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
        public async Task<IActionResult> EditUserData(UserDataViewModel userData)
        {
            await userService.EditUserDataAsync(userData);
            return NoContent();
        }

        [HttpPut("Password")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> EditUserPassword(UserChangePasswordViewModel userData)
        {
            await userService.EditUserPasswordAsync(userData);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("Address")]
        public async Task<IActionResult> EditUserAddressData(UserAddressViewModel userData)
        {
            await userService.EditUserAddressDataAsync(userData);
            return NoContent();
        }

        [HttpPost]
        [Route("Order")]
        public async Task<IActionResult> CreateNewOrder(OrderPostViewModel orderModel)
        {
            await orderService.CreateNewOrderAsync(orderModel);
            return Created();
        }

        [HttpGet]
        [Route("Order")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetUserOrdersAsync([FromQuery] OrderFiltersViewModel orderFilters)
        {
            var orders = await orderService.GetUserOrdersAsync(orderFilters);
            return Ok(orders);
        }

        [HttpGet]
        [Route("Order/{id}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<OrderDetailsViewModel>> GetUserOrderByIdAsync(int id)
        {
            var order = await orderService.GetUserOrderByIdAsync(id);
            return Ok(order);
        }
    }
}
