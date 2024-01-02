//using BookStoreAPI.BusinessLogic.OrderLogic;
//using BookStoreData.Data;
//using BookStoreAPI.Helpers.BaseController;
//using BookStoreData.Models.Orders;
//using BookStoreViewModels.ViewModels.Customers;
//using BookStoreViewModels.ViewModels.Customers.Address;
//using BookStoreViewModels.ViewModels.Orders;
//using BookStoreViewModels.ViewModels.Orders.Dictionaries;
//using BookStoreViewModels.ViewModels.Payments;
//using BookStoreViewModels.ViewModels.Payments.Dictionaries;
//using BookStoreViewModels.ViewModels.Products.BookItems;
//using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;
//using BookStoreViewModels.ViewModels.Shippings;
//using BookStoreViewModels.ViewModels.Shippings.Dictionaries;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using BookStoreData.Models.Accounts;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;

//namespace BookStoreAPI.Controllers.Orders
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    //[Authorize(Roles = $"{UserRoles.Employee}, {UserRoles.Admin}")]
//    public class OrderController : CRUDController<Order, OrderPostForView, OrderForView, OrderDetailsForView>
//    {
//        private readonly UserManager<User> _userManager;
//        public OrderController(BookStoreContext context, UserManager<User> userManager) : base(context)
//        {
//            _userManager = userManager;
//        }

//        [HttpGet]
//        [Route("Get-User-Orders")]
//        [Authorize(Roles = UserRoles.User)]
//        public async Task<ActionResult<IEnumerable<OrderDetailsWWWForView>>> GetUserOrders([FromQuery] OrderFiltersForView orderFilters)
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (userId == null)
//            {
//                return BadRequest("Nie można znaleźć identyfikatora użytkownika.");
//            }

//            var user = await _userManager.FindByIdAsync(userId);

//            if (user == null)
//            {
//                return NotFound("Nie można znaleźć użytkownika o podanym identyfikatorze.");
//            }

//            var customer = await _context.Customer.FirstOrDefaultAsync(x => x.IsActive && x.Id == user.CustomerID);

//            if (customer == null)
//            {
//                return NotFound("Nie znaleziono danych klienta.");
//            }

//            return await OrderB.GetUserOrders(customer, orderFilters, _context);
//        }

//        protected override async Task<ActionResult<OrderDetailsForView?>> GetCustomEntityByIdAsync(int id)
//        {
//            return await _context.Order
//                .Include(x => x.OrderItems)
//                    .ThenInclude(x => x.BookItem)
//                    .ThenInclude(x => x.Book)
//                    .ThenInclude(x => x.BookAuthors)
//                .Include(x => x.OrderItems)
//                    .ThenInclude(x => x.BookItem)
//                    .ThenInclude(x => x.Book)
//                    .ThenInclude(x => x.BookImages)
//                .Include(x => x.Customer)
//                .Include(x => x.OrderStatus)
//                .Include(x => x.DeliveryMethod)
//                .Include(x => x.Payment)
//                    .ThenInclude(x => x.TransactionStatus)
//                .Include(x => x.Payment)
//                    .ThenInclude(x => x.PaymentMethod)
//                .Include(x => x.Shipping)
//                    .ThenInclude(x => x.ShippingStatus)
//                .Include(x => x.Shipping)
//                    .ThenInclude(x => x.Address)
//                    .ThenInclude(x => x.City)
//                .Include(x => x.Shipping)
//                    .ThenInclude(x => x.Address)
//                    .ThenInclude(x => x.Country)
//                .Where(x => x.Id == id && x.IsActive == true)
//                .Select(element => new OrderDetailsForView()
//                {
//                    Id = element.Id,
//                    OrderDate = element.OrderDate,
//                    DeliveryMethod = new DeliveryMethodForView
//                    {
//                        Id = (int)element.DeliveryMethodID,
//                        Name = element.DeliveryMethod.Name,
//                        Price = element.DeliveryMethod.Price
//                    },
//                    OrderStatus = new OrderStatusForView
//                    {
//                        Id = (int)element.OrderStatusID,
//                        Name = element.OrderStatus.Name
//                    },
//                    Payment = new PaymentDetailsForView
//                    {
//                        Id = (int)element.PaymentID,
//                        Amount = element.Payment.Amount,
//                        PaymentDate = element.Payment.Date,
//                        PaymentMethod = new PaymentMethodForView
//                        {
//                            Id = element.Payment.PaymentMethodID,
//                            Name = element.Payment.PaymentMethod.Name
//                        },
//                        TransactionStatus = new TransactionStatusForView
//                        {
//                            Id = element.Payment.TransactionStatusID,
//                            Name = element.Payment.TransactionStatus.Name
//                        }
//                    },
//                    Shipping = new ShippingDetailsForView
//                    {
//                        Id = (int)element.ShippingID,
//                        ShippingDate = element.Shipping.ShippingDate,
//                        DeliveryDate = element.Shipping.DeliveryDate,
//                        ShippingAddress = new AddressDetailsForView
//                        {
//                            Id = (int)element.Shipping.AddressID,
//                            CityName = element.Shipping.Address.City.Name,
//                            CountryName = element.Shipping.Address.Country.Name,
//                            Street = element.Shipping.Address.Street,
//                            StreetNumber = element.Shipping.Address.StreetNumber,
//                            HouseNumber = element.Shipping.Address.HouseNumber,
//                            Postcode = element.Shipping.Address.Postcode,
//                            CityID = element.Shipping.Address.CityID,
//                            CountryID = element.Shipping.Address.CountryID,
//                        },
//                        ShippingStatus = new ShippingStatusForView
//                        {
//                            Id = (int)element.Shipping.ShippingStatusID,
//                            Name = element.Shipping.ShippingStatus.Name
//                        }
//                    },
//                    Customer = new CustomerForOrderForView
//                    {
//                        Id = (int)element.CustomerID,
//                        Name = element.Customer.Name,
//                        Surname = element.Customer.Surname,
//                    },
//                    ListOfOrderItems = element.OrderItems
//                    .Where(x => x.IsActive == true)
//                    .Select(x => new OrderItemsDetailsForView
//                    {
//                        Id = x.Id,
//                        Quantity = x.Quantity,
//                        BruttoPrice = x.BruttoPrice,
//                        BookItemID = x.BookItemID,
//                        OrderID = x.OrderID,
//                        ItemForOrder = new BookItemsForOrderDetailsForView
//                        {
//                            Id = x.BookItemID,
//                            Title = x.BookItem.Book.Title,
//                            AuthorsName = x.BookItem.Book.BookAuthors
//                                    .Where(y => y.BookID == x.BookItem.BookID == y.IsActive == true)
//                                    .Select(y => new AuthorsForView
//                                    {
//                                        Id = y.Author.Id,
//                                        Name = y.Author.Name,
//                                        Surname = y.Author.Surname
//                                    }).ToList(),
//                            ImageURL = x.BookItem.Book.BookImages
//                                    .Where(y => y.BookID == x.BookItem.BookID && y.IsActive == true)
//                                    .Select(y => y.Image.ImageURL)
//                                    .First(),
//                        }
//                    }).ToList()
//                })
//                .FirstAsync();
//        }
//        protected override async Task<ActionResult<IEnumerable<OrderForView>>> GetAllEntitiesCustomAsync()
//        {
//            return await _context.Order
//                .Include(x => x.OrderStatus)
//                .Where(x => x.IsActive == true)
//                .Select(x => new OrderForView
//                {
//                    Id = x.Id,
//                    OrderStatusName = x.OrderStatus.Name,
//                    CustomerID = x.CustomerID,
//                    OrderDate = x.OrderDate
//                })
//                .ToListAsync();
//        }
//        protected override async Task<IActionResult> CreateEntityCustomAsync(OrderPostForView entity)
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (userId == null)
//            {
//                return BadRequest("Nie można znaleźć identyfikatora użytkownika.");
//            }

//            var user = await _userManager.FindByIdAsync(userId);

//            if (user == null)
//            {
//                return NotFound("Nie można znaleźć użytkownika o podanym identyfikatorze.");
//            }

//            var customer = await _context.Customer.FirstOrDefaultAsync(x => x.IsActive && x.Id == user.CustomerID);

//            if (customer == null)
//            {
//                return NotFound("Nie znaleziono danych klienta.");
//            }

//            entity.CustomerID = customer.Id;
//            return await OrderB.ConvertEntityPostForViewAndSave<OrderB>(entity, _context);
//        }
//        protected override async Task UpdateEntityCustomAsync(Order oldEntity, OrderPostForView updatedEntity)
//        {
//            await OrderB.ConvertEntityPostForViewAndUpdate<OrderB>(oldEntity, updatedEntity, _context);
//        }
//        protected override async Task<IActionResult> DeleteEntityCustomAsync(Order entity)
//        {
//            return await OrderB.DeactivateEntityAndSave<OrderB>(entity, _context);
//        }
//    }
//}
