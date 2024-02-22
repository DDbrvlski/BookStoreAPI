using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Customers;
using BookStoreAPI.Services.Email;
using BookStoreAPI.Services.Users;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreDto.Dtos.Accounts.Account;
using BookStoreDto.Dtos.Accounts.User;
using BookStoreDto.Dtos.Customers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreAPI.Services.Auth
{
    public interface IAuthService
    {
        Task CheckTokenValidity(string token);
        string DecodeToken(string token);
        Task<string> Login(AccountLoginDto loginData);
        Task Register(AccountRegisterDto registerData);
        Task ConfirmEmail(string userId, string token);
        Task ForgotPassword(AccountForgotPasswordDto forgotPasswordModel);
        Task ResetPassword(AccountResetPasswordDto resetPasswordModel);
    }

    public class AuthService
            (UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUserContextService userContextService,
            IUserService userService,
            ICustomerService customerService,
            IEmailSenderService emailSender,
            BookStoreContext context) : IAuthService
    {
        private readonly SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTKey:Secret"]));
        private readonly long tokenExpiryTimeInHours = Convert.ToInt64(configuration["JWTKey:TokenExpiryTimeInHour"]);

        public async Task<string> Login(AccountLoginDto loginData)
        {
            if (loginData == null)
            {
                throw new UnauthorizedException("Wprowadzono błędne dane logowania.");
            }

            var user = await userContextService.GetUserByDataAsync(x => x.Email == loginData.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginData.Password))
            {
                throw new UnauthorizedException("Wprowadzono błędne dane logowania.");
            }

            if (loginData.Audience == "CMS")
            {
                var isUserInUserRole = await userManager.IsInRoleAsync(user, "User");
                if (isUserInUserRole)
                {
                    throw new BadRequestException("Brak możliwości zalogowania się do systemu pracowników.");
                }
            }

            //if (!user.EmailConfirmed)
            //{
            //    throw new UnauthorizedException("Adres email nie został potwierdzony.");
            //}

            string token = await GenerateTokenAsync(user, loginData.Audience);
            return token;
        }
        public async Task Register(AccountRegisterDto registerData)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    await userService.ValidateUserFieldsAsync(registerData.Username, registerData.Email);

                    var customer = await customerService
                        .CreateCustomerAsync(new CustomerPostDto()
                        {
                            Name = registerData.Name,
                            Surname = registerData.Surname,
                            Email = registerData.Email,
                            PhoneNumber = registerData.PhoneNumber,
                            IsSubscribed = (bool)registerData.IsSubscribed
                        });

                    var user = await userService
                        .CreateUserAsync(new UserPostDto()
                        {
                            CustomerId = customer.Id,
                            Email = registerData.Email,
                            Password = registerData.Password,
                            PhoneNumber = registerData.PhoneNumber,
                            Username = registerData.Username,
                            RoleName = registerData.RoleName
                        });

                    //var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var urlCodedEmailConfirmationToken = CodeTokenToURL(emailConfirmationToken);
                    //await emailSender.ConfirmEmailEmail(urlCodedEmailConfirmationToken, user);

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task CheckTokenValidity(string token)
        {
            if (token.IsNullOrEmpty())
            {
                throw new BadRequestException("Token jest pusty.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            if (jwtSecurityToken.ValidTo < DateTime.UtcNow.AddSeconds(10))
            {
                throw new BadRequestException("Ważność tokena wygasła.");
            }
        }
        public async Task ConfirmEmail(string userId, string token)
        {
            var user = await userContextService.GetUserByDataAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new BadRequestException("Wystąpił błąd z pobieraniem użytkownika.");
            }
            if (user.EmailConfirmed)
            {
                throw new BadRequestException("Email został już potwierdzony.");
            }

            var decodedToken = DecodeToken(token);

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Wystąpił błąd z potwierdzaniem emaila.");
            }
        }
        public async Task ForgotPassword(AccountForgotPasswordDto forgotPasswordModel)
        {
            var user = await userContextService.GetUserByDataAsync(x => x.Email == forgotPasswordModel.Email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                await emailSender.ResetPasswordEmail(token, user);
            }
        }
        public async Task ResetPassword(AccountResetPasswordDto resetPasswordModel)
        {
            var user = await userContextService.GetUserByDataAsync(x => x.Id == resetPasswordModel.UserId);
            if (user == null)
            {
                throw new BadRequestException("Wystąpił błąd z pobieraniem użytkownika.");
            }

            if (resetPasswordModel.Password != resetPasswordModel.ConfirmPassword)
            {
                throw new BadRequestException("Podane hasła się różnią.");
            }

            var result = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Wystąpił błąd podczas zmiany hasła.");
            }
        }
        
        private async Task<string> GenerateTokenAsync(User user, string audience)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var roleName in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roleName));
                var roleClaims = await roleManager.GetClaimsAsync(await roleManager.FindByNameAsync(roleName));
                authClaims.AddRange(roleClaims);
            }

            //authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration["JWTKey:ValidIssuer"],
                Audience = configuration["Audiences:" + audience],
                Expires = DateTime.UtcNow.AddHours(tokenExpiryTimeInHours),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string CodeTokenToURL(string token)
        {
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
        }
        public string DecodeToken(string token)
        {
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(token);
            return Encoding.UTF8.GetString(tokenDecodedBytes);
        }
    }
}
