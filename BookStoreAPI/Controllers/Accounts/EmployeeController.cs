using BookStoreAPI.Services.Employees;
using BookStoreDto.Dtos.Admin;
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
        public async Task<ActionResult<EmployeeDetailsDto>> GetEmployeeDetailsAsync()
        {
            var employeeData = await employeeService.GetEmployeeDataAsync();
            return Ok(employeeData);
        }
    }
}
