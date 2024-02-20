using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Admin;
using BookStoreAPI.Services.Auth;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Admin;
using BookStoreDto.Dtos.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController
        (IAuthService authService,
        IAdminPanelService adminPanelService) 
        : ControllerBase
    {
        [HttpGet]
        [Route("Employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDataDto>>> GetEmployees()
        {
            var employees = await adminPanelService.GetEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet]
        [Route("Employees/{employeeId}")]
        public async Task<ActionResult<EmployeeDetailsDto>> GetEmployeeDetails(string employeeId)
        {
            var employee = await adminPanelService.GetEmployeeDetailsAsync(employeeId);
            return Ok(employee);
        }

        [HttpPost]
        [Route("Register/Employee")]
        public async Task<IActionResult> RegisterEmployee(AccountRegisterDto registerData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            registerData.RoleName ??= "Employee";
            await authService.Register(registerData);

            return Ok();
        }

        [HttpPost]
        [Route("Employee/Edit")]
        public async Task <IActionResult> EditEmployee(EmployeeDataEditDto employeeDetails)
        {
            await adminPanelService.EditEmployeeDataAsync(employeeDetails);
            return NoContent();
        }

        [HttpDelete]
        [Route("Employee/Delete")]
        public async Task<IActionResult> DeactivateEmployee(string userId)
        {
            await adminPanelService.DeactivateUserAsync(userId);
            return NoContent();
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            var roles = await adminPanelService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpPost]
        [Route("Roles")]
        public async Task<IActionResult> AddNewRoles(string roleName)
        {
            await adminPanelService.AddNewRole(roleName);
            return Ok();
        }

        [HttpDelete]
        [Route("Roles/{roleName}")]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            await adminPanelService.RemoveRole(roleName);
            return NoContent();
        }

        [HttpGet]
        [Route("Roles/Claims")]
        public async Task<ActionResult<RoleClaimsPostDto>> GetRoleClaims(string roleName)
        {
            var roleClaims = await adminPanelService.GetAllRoleClaimsAsync(roleName);
            return Ok(roleClaims);
        }

        [HttpPost]
        [Route("Roles/Claims")]
        public async Task<IActionResult> AddClaimsToRole(RoleClaimsPostDto roleClaims)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await adminPanelService.AddClaimsToRole(roleClaims);
            return NoContent();
        }

        [HttpGet]
        [Route("Claims")]
        public async Task<ActionResult<IEnumerable<Claims>>> GetClaims()
        {
            var claims = await adminPanelService.GetAllClaimsAsync();
            return Ok(claims);
        }

        [HttpPost]
        [Route("Claims")]
        public async Task<IActionResult> AddNewClaim(List<string> claims)
        {
            await adminPanelService.AddClaims(claims);
            return NoContent();
        }

        [HttpDelete]
        [Route("Claims")]
        public async Task<IActionResult> RemoveClaim(string claimName)
        {
            await adminPanelService.RemoveClaims(claimName);
            return NoContent();
        }

        [HttpGet]
        [Route("ClaimValues")]
        public async Task<ActionResult<ClaimValues>> GetClaimValues()
        {
            var claimValues = await adminPanelService.GetAllClaimValuesAsync();
            return Ok(claimValues);
        }
    }
}
