using BookStoreAPI.Services.Employees;
using BookStoreViewModels.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        [HttpGet]
        [Route("Data")]
        [Authorize]
        public async Task<ActionResult<EmployeeDetailsViewModel>> GetEmployeeDetailsAsync()
        {
            var employeeData = await employeeService.GetEmployeeDataAsync();
            return Ok(employeeData);
        }
    }
}
