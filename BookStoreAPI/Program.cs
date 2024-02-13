using BookStoreAPI.Helpers.BaseService;
using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Addresses;
using BookStoreAPI.Services.Admin;
using BookStoreAPI.Services.Auth;
using BookStoreAPI.Services.Availability;
using BookStoreAPI.Services.BookItems;
using BookStoreAPI.Services.Books;
using BookStoreAPI.Services.Books.Dictionaries;
using BookStoreAPI.Services.CMS;
using BookStoreAPI.Services.Customers;
using BookStoreAPI.Services.Discounts.DiscountCodes;
using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreAPI.Services.Email;
using BookStoreAPI.Services.Invoices;
using BookStoreAPI.Services.Library;
using BookStoreAPI.Services.Media;
using BookStoreAPI.Services.Notifications;
using BookStoreAPI.Services.Orders;
using BookStoreAPI.Services.PageElements;
using BookStoreAPI.Services.Payments;
using BookStoreAPI.Services.Policies;
using BookStoreAPI.Services.Rentals;
using BookStoreAPI.Services.Reviews;
using BookStoreAPI.Services.Statistic;
using BookStoreAPI.Services.Stock;
using BookStoreAPI.Services.Supplies;
using BookStoreAPI.Services.Users;
using BookStoreAPI.Services.Wishlists;
using BookStoreBusinessLogic.BusinessLogic.BookReviews;
using BookStoreBusinessLogic.BusinessLogic.CMS;
using BookStoreBusinessLogic.BusinessLogic.Discounts;
using BookStoreData.Data;
using BookStoreData.Models.Accounts;
using BookStoreViewModels.ViewModels.Accounts.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Text;

namespace BookStoreAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<BookStoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreContext") ?? throw new InvalidOperationException("Connection string 'BookStoreContext' not found.")));


            QuestPDF.Settings.License = LicenseType.Community;
            //var model = InvoiceService.CreateInvoice(43);
            //var document = new InvoiceDocument();

            //document.GeneratePdf("hello.pdf");

            //use the following invocation
            //document.ShowInPreviewer();

            //optionally, you can specify an HTTP port to communicate with the previewer host(default is 12500)
            //document.ShowInPreviewer(12345);

            builder.Configuration.AddJsonFile("appsettings.json");

            var audiences = builder.Configuration.GetSection("Audiences").Get<Dictionary<string, string>>();
            var emailConfiguration = builder.Configuration.GetSection("EmailConfiguration").Get<AccountEmailConfigurationViewModel>();
            builder.Services.AddSingleton(emailConfiguration);
            builder.Services.AddScoped<PolicyService>();
            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            builder.Services.AddTransient<IAdminPanelService, AdminPanelService>();
            builder.Services.AddTransient<IInvoiceService, InvoiceService>();
            builder.Services.AddTransient<IAvailabilityService, AvailabilityService>();
            builder.Services.AddTransient<IBookDiscountService, BookDiscountService>();
            builder.Services.AddTransient<IStatisticsLogic, StatisticsLogic>();
            builder.Services.AddTransient<ISupplyService, SupplyService>();
            builder.Services.AddTransient<ISupplierService, SupplierService>();
            builder.Services.AddTransient<IStatisticsService, StatisticsService>();
            builder.Services.AddTransient<ISupplyGoodsService, SupplyGoodsService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
            builder.Services.AddTransient<IBookReviewLogic, BookReviewLogic>();
            builder.Services.AddTransient<ICMSService, CMSService>();
            builder.Services.AddTransient<IDiscountLogic, DiscountLogic>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IAuthorService, AuthorService>();
            builder.Services.AddTransient<IStockAmountService, StockAmountService>();
            builder.Services.AddTransient<IRentalService, RentalService>();
            builder.Services.AddTransient<INewsService, NewsService>();
            builder.Services.AddTransient<IBookItemService, BookItemService>();
            builder.Services.AddTransient<IUserContextService, UserContextService>();
            builder.Services.AddTransient<IFooterLinkService, FooterLinkService>();
            builder.Services.AddTransient<IContactService, ContactService>();
            builder.Services.AddTransient<IDiscountBannerService, DiscountBannerService>();
            builder.Services.AddTransient<IDiscountService, DiscountService>();
            builder.Services.AddTransient<IDiscountCodeService, DiscountCodeService>();
            builder.Services.AddTransient<IBannerService, BannerService>();
            builder.Services.AddTransient<ICategoryElementService, CategoryElementService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddTransient<IBookReviewService, BookReviewService>();
            builder.Services.AddTransient<INewsletterService, NewsletterService>();
            builder.Services.AddTransient<ILibraryService, LibraryService>();
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IWishlistService, WishlistService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<IImageService, ImageService>();
            builder.Services.AddTransient<IBookService, BookService>();
            builder.Services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetRequiredService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
                .AddEntityFrameworkStores<BookStoreContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudiences = builder.Configuration.GetSection("JWTKey:ValidAudience").Get<string[]>(),
                        ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]))
                    };
                });

            var claimService = builder.Services.BuildServiceProvider().GetService<PolicyService>();
            var policyClaims = await claimService.CreateAuthorizationPoliciesAsync();

            builder.Services.AddAuthorization(options =>
            {
                foreach(var claim in policyClaims)
                {
                    options.AddPolicy(claim.PolicyName, c =>
                    {
                        c.RequireClaim(claim.ClaimName, claim.ClaimValue);
                        c.RequireRole("Admin");
                    });
                }
            });

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            //builder.Services.AddProblemDetails();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowSpecificOrigin", builder =>
            //    {
            //        builder.WithOrigins("http://localhost:3000", "http://localhost:3001")
            //               .AllowAnyHeader()
            //               .AllowAnyMethod();
            //    });
            //});


            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BookStoreAPI", Version = "v1" });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                  Enter 'Bearer' [space] and then your token in the text input below. 
                  Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                  new OpenApiSecurityScheme
                  {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                });
            });


            var app = builder.Build();
            //app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}