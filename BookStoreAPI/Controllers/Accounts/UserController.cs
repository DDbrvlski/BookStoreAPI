using BookStoreAPI.Services.Users;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Accounts.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.User)]
    public class UserController
        (IUserService userService)
        : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<UserDataViewModel>> GetUserData()
        {
            var user = await userService.GetUserDataAsync();
            return Ok(user);
        }

        [HttpGet("Address")]
        public async Task<ActionResult<IEnumerable<UserAddressViewModel>>> GetUserDataAddress()
        {
            var userAddress = await userService.GetUserAddressDataAsync();
            return Ok(userAddress);
        }

        [HttpDelete]
        public async Task<IActionResult> DeactivateUserAccount()
        {
            await userService.DeactivateUserAccountAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> EditUserData(UserDataViewModel userData)
        {
            await userService.EditUserDataAsync(userData);
            return NoContent();
        }

        [HttpPut("Password")]
        public async Task<IActionResult> EditUserPassword(UserChangePasswordViewModel userData)
        {
            await userService.EditUserPasswordAsync(userData);
            return NoContent();
        }

        [HttpPost]
        [Route("Address")]
        public async Task<IActionResult> EditUserAddressData(UserAddressViewModel userData)
        {
            await userService.EditUserAddressDataAsync(userData);
            return NoContent();
        }
    }
}
