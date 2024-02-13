﻿using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Admin;
using BookStoreAPI.Services.Auth;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Admin;
using BookStoreViewModels.ViewModels.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController
        (IAuthService authService,
        IAdminPanelService adminPanelService) 
        : ControllerBase
    {
        [HttpPost]
        [Route("Register/Employee")]
        public async Task<IActionResult> RegisterEmployee(AccountRegisterViewModel registerData)
        {
            if (!ModelState.IsValid)
            {
                throw new AccountException("Wprowadzono błędne dane.");
            }

            await authService.Register(registerData);

            return Ok();
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
        [Route("Roles")]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            await adminPanelService.RemoveRole(roleName);
            return NoContent();
        }

        [HttpGet]
        [Route("Roles/Claims")]
        public async Task<ActionResult<RoleClaimsPost>> GetRoleClaims(string roleName)
        {
            var roleClaims = await adminPanelService.GetAllRoleClaimsAsync(roleName);
            return Ok(roleClaims);
        }

        [HttpPost]
        [Route("Roles/Claims")]
        public async Task<IActionResult> AddClaimsToRole(RoleClaimsPost roleClaims)
        {
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