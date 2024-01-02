using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Auth;
using BookStoreViewModels.ViewModels.Accounts.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController
        (IAuthService authService) 
        : ControllerBase
    {

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(AccountLoginViewModel loginData)
        {
            var token = await authService.Login(loginData);

            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(AccountRegisterViewModel registerData)
        {
            if (!ModelState.IsValid)
            {
                throw new AccountException("Wprowadzono błędne dane.");
            }

            await authService.Register(registerData);

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            await authService.ConfirmEmail(userId, token);
            return Ok("Pomyślnie potwierdzono adres email.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(AccountForgotPasswordViewModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                throw new AccountException("Wprowadzono niepoprawny adres email.");
            }

            await authService.ForgotPassword(forgotPasswordModel);
            return Ok("Jeżeli istnieje konto z podanym emailem, wyślemy na niego wiadomość.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(AccountResetPasswordViewModel resetPasswordModel)
        {
            await authService.ResetPassword(resetPasswordModel);
            return Ok("Pomyślnie zresetowano hasło.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CheckTokenValidity")]
        public async Task<IActionResult> CheckTokenValidity(string token)
        {
            await authService.CheckTokenValidity(token);
            return Ok();
        }

    }
}
