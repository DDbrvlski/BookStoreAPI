using BookStoreAPI.Helpers;
using BookStoreAPI.Services.Addresses;
using BookStoreAPI.Services.BookItems;
using BookStoreAPI.Services.Customers;
using BookStoreAPI.Services.Discounts.DiscountCodes;
using BookStoreAPI.Services.Discounts.Discounts;
using BookStoreAPI.Services.Payments;
using BookStoreAPI.Services.Stock;
using BookStoreBusinessLogic.BusinessLogic.Discounts;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreData.Models.Orders;
using BookStoreData.Models.Products.BookItems;
using BookStoreViewModels.ViewModels.Customers;
using BookStoreViewModels.ViewModels.Customers.Address;
using BookStoreViewModels.ViewModels.Invoices;
using BookStoreViewModels.ViewModels.Orders;
using BookStoreViewModels.ViewModels.Orders.Dictionaries;
using BookStoreViewModels.ViewModels.Payments;
using BookStoreViewModels.ViewModels.Payments.Dictionaries;
using BookStoreViewModels.ViewModels.Products.BookItems;
using BookStoreViewModels.ViewModels.Products.Books.Dictionaries;
using BookStoreViewModels.ViewModels.Stock;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BookStoreAPI.Services.Orders
{
    public interface IOrderService
    {
        Task CreateNewOrderAsync(OrderPostViewModel orderModel);
        Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync();
        Task<OrderDetailsViewModel> GetOrderByIdAsync(int orderId);
        Task<OrderDetailsViewModel> GetUserOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderViewModel>> GetUserOrdersAsync(OrderFiltersViewModel orderFilters);
        Task<InvoiceDataViewModel> GetUserOrderForInvoiceByOrderIdAsync(int orderId);
        Task<OrderDiscountCheckViewModel> ApplyDiscountCodeToOrderAsync(OrderDiscountCheckViewModel discountCode);
    }

    public class OrderService
                (BookStoreContext context,
                ICustomerService customerService,
                IAddressService addressService,
                IPaymentService paymentService,
                IDiscountCodeService discountCodeService,
                IBookItemService bookItemService,
                IBookDiscountService bookDiscountService,
                IStockAmountService stockAmountService) : IOrderService
    {
        public async Task<OrderDetailsViewModel> GetOrderByIdAsync(int orderId)
        {
            var order = await context.Order
                .Where(x => x.Id == orderId && x.IsActive)
                .Select(element => new OrderDetailsViewModel()
                {
                    Id = element.Id,
                    OrderDate = element.OrderDate,
                    DeliveryMethod = new DeliveryMethodViewModel
                    {
                        Id = (int)element.DeliveryMethodID,
                        Name = element.DeliveryMethod.Name,
                        Price = element.DeliveryMethod.Price
                    },
                    OrderStatus = new OrderStatusViewModel
                    {
                        Id = (int)element.OrderStatusID,
                        Name = element.OrderStatus.Name
                    },
                    Payment = new PaymentDetailsViewModel
                    {
                        Id = (int)element.PaymentID,
                        Amount = element.Payment.Amount,
                        PaymentDate = element.Payment.PaymentDate,
                        PaymentMethod = new PaymentMethodViewModel
                        {
                            Id = (int)element.Payment.PaymentMethodID,
                            Name = element.Payment.PaymentMethod.Name
                        },
                        TransactionStatus = new TransactionStatusViewModel
                        {
                            Id = (int)element.Payment.TransactionStatusID,
                            Name = element.Payment.TransactionStatus.Name
                        }
                    },
                    Customer = new CustomerShortDetailsViewModel
                    {
                        Id = (int)element.CustomerID,
                        Name = element.Customer.Name,
                        Surname = element.Customer.Surname,
                        Email = element.Customer.Email,
                    },
                    OrderItems = element.OrderItems
                    .Where(x => x.IsActive && x.OrderID == element.Id)
                    .Select(x => new OrderItemDetailsViewModel
                    {
                        Id = (int)x.BookItemID,
                        Quantity = x.Quantity,
                        TotalBruttoPrice = x.TotalBruttoPrice,
                        BruttoPrice = x.BruttoPrice,
                        BookTitle = x.BookItem.Book.Title,
                        EditionName = x.BookItem.Edition.Name,
                        FileFormatName = x.BookItem.FileFormat.Name,
                        FormName = x.BookItem.Form.Name,
                        ImageURL = x.BookItem.Book.BookImages
                            .First(x => x.Image.Position == 1).Image.ImageURL,
                        Authors = x.BookItem.Book.BookAuthors
                            .Where(y => y.IsActive)
                            .Select(y => new AuthorViewModel()
                            {
                                Id = (int)y.AuthorID,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList()
                    }).ToList()
                })
                .FirstAsync();

            order.TotalBruttoPrice = (decimal)order.OrderItems.Sum(x => x.TotalBruttoPrice);
            return order;
        }
        public async Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync()
        {
            return await context.Order
                .Where(x => x.IsActive)
                .Select(x => new OrderViewModel()
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate,
                    TotalBruttoPrice = x.OrderItems
                    .Where(y => y.IsActive && y.OrderID == x.Id)
                    .Sum(y => y.Quantity * y.BruttoPrice),
                    OrderItems = x.OrderItems
                    .Where(y => y.IsActive && y.OrderID == x.Id)
                    .Select(y => new OrderItemDetailsViewModel
                    {
                        Id = y.Id,
                        Quantity = y.Quantity,
                        TotalBruttoPrice = y.TotalBruttoPrice,
                        BruttoPrice = y.BruttoPrice,
                        BookTitle = y.BookItem.Book.Title,
                        EditionName = y.BookItem.Edition.Name,
                        FormName = y.BookItem.Form.Name,
                        ImageURL = y.BookItem.Book.BookImages
                            .First(z => z.Image.Position == 1).Image.ImageURL,
                        Authors = y.BookItem.Book.BookAuthors
                            .Where(z => z.IsActive)
                            .Select(z => new AuthorViewModel()
                            {
                                Id = (int)z.AuthorID,
                                Name = z.Author.Name,
                                Surname = z.Author.Surname,
                            }).ToList()
                    }).ToList()
                }).ToListAsync();
        }
        public async Task<OrderDetailsViewModel> GetUserOrderByIdAsync(int orderId)
        {
            Customer customer = await customerService.GetCustomerByTokenAsync();

            return await context.Order
                .Where(x => x.CustomerID == customer.Id && x.Id == orderId && x.IsActive)
                .Select(element => new OrderDetailsViewModel()
                {
                    Id = element.Id,
                    OrderDate = element.OrderDate,
                    DeliveryMethod = new DeliveryMethodViewModel
                    {
                        Id = (int)element.DeliveryMethodID,
                        Name = element.DeliveryMethod.Name,
                        Price = element.DeliveryMethod.Price
                    },
                    OrderStatus = new OrderStatusViewModel
                    {
                        Id = (int)element.OrderStatusID,
                        Name = element.OrderStatus.Name
                    },
                    Payment = new PaymentDetailsViewModel
                    {
                        Id = (int)element.PaymentID,
                        Amount = element.Payment.Amount,
                        PaymentDate = element.Payment.PaymentDate,
                        PaymentMethod = new PaymentMethodViewModel
                        {
                            Id = (int)element.Payment.PaymentMethodID,
                            Name = element.Payment.PaymentMethod.Name
                        },
                        TransactionStatus = new TransactionStatusViewModel
                        {
                            Id = (int)element.Payment.TransactionStatusID,
                            Name = element.Payment.TransactionStatus.Name
                        }
                    },
                    Customer = new CustomerShortDetailsViewModel
                    {
                        Id = (int)element.CustomerID,
                        Name = element.Customer.Name,
                        Surname = element.Customer.Surname,
                        Email = element.Customer.Email,
                    },
                    OrderItems = element.OrderItems
                    .Where(x => x.IsActive && x.OrderID == element.Id)
                    .Select(x => new OrderItemDetailsViewModel
                    {
                        Id = (int)x.BookItemID,
                        Quantity = x.Quantity,
                        TotalBruttoPrice = x.Quantity * x.BruttoPrice,
                        BruttoPrice = x.BruttoPrice,
                        BookTitle = x.BookItem.Book.Title,
                        EditionName = x.BookItem.Edition.Name,
                        FormName = x.BookItem.Form.Name,
                        ImageURL = x.BookItem.Book.BookImages
                            .First(x => x.Image.Position == 1).Image.ImageURL,
                        Authors = x.BookItem.Book.BookAuthors
                            .Where(y => y.IsActive)
                            .Select(y => new AuthorViewModel()
                            {
                                Id = (int)y.AuthorID,
                                Name = y.Author.Name,
                                Surname = y.Author.Surname,
                            }).ToList()
                    }).ToList()
                })
                .FirstAsync();
        }
        public async Task<IEnumerable<OrderViewModel>> GetUserOrdersAsync(OrderFiltersViewModel orderFilters)
        {
            Customer customer = await customerService.GetCustomerByTokenAsync();

            var orders = context.Order
                .Where(x => x.CustomerID == customer.Id && x.IsActive)
                .ApplyOrderFilters(orderFilters);

            return await orders.Select(x => new OrderViewModel()
            {
                Id = x.Id,
                OrderDate = x.OrderDate,
                TotalBruttoPrice = x.OrderItems
                    .Where(y => y.IsActive && y.OrderID == x.Id)
                    .Sum(y => y.Quantity * y.BruttoPrice),
                OrderItems = x.OrderItems
                    .Where(y => y.IsActive && y.OrderID == x.Id)
                    .Select(y => new OrderItemDetailsViewModel
                    {
                        Id = (int)y.BookItemID,
                        Quantity = y.Quantity,
                        TotalBruttoPrice = y.Quantity * y.BruttoPrice,
                        FileFormatName = y.BookItem.FileFormat.Name,
                        BruttoPrice = y.BruttoPrice,
                        BookTitle = y.BookItem.Book.Title,
                        EditionName = y.BookItem.Edition.Name,
                        FormName = y.BookItem.Form.Name,
                        ImageURL = y.BookItem.Book.BookImages
                            .First(z => z.Image.Position == 1).Image.ImageURL,
                        Authors = y.BookItem.Book.BookAuthors
                            .Where(z => z.IsActive)
                            .Select(z => new AuthorViewModel()
                            {
                                Id = (int)z.AuthorID,
                                Name = z.Author.Name,
                                Surname = z.Author.Surname,
                            }).ToList()
                    }).ToList()
            }).ToListAsync();
        }
        public async Task CreateNewOrderAsync(OrderPostViewModel orderModel)
        {
            Customer customer = new Customer();

            if (orderModel.CustomerGuest == null)
            {
                customer = await customerService.GetCustomerByTokenAsync();
            }
            else
            {
                customer = await customerService
                    .CreateCustomerAsync(new CustomerPostViewModel()
                    {
                        Name = orderModel.CustomerGuest.Name,
                        Surname = orderModel.CustomerGuest.Surname,
                        Email = orderModel.CustomerGuest.Email,
                    });
            }

            orderModel.InvoiceAddress.AddressTypeID = 3;

            List<BaseAddressViewModel> orderAddresses = [orderModel.InvoiceAddress];
            if (orderModel.DeliveryAddress != null)
            {
                orderModel.DeliveryAddress.AddressTypeID = 4;
                orderAddresses.Add(orderModel.DeliveryAddress);
            }

            orderModel.CartItems = await SetOriginalPriceForItems(orderModel.CartItems);
            orderModel.CartItems = await bookDiscountService.ApplyDiscount(orderModel.CartItems);
            if (orderModel.DiscountCodeID != null)
            {
                orderModel.CartItems = await discountCodeService.ApplyDiscountCodeToCartItemsAsync(orderModel.CartItems, (int)orderModel.DiscountCodeID);
            }

            decimal totalOrderBruttoPrice = 0;
            foreach (var item in orderModel.CartItems)
            {
                totalOrderBruttoPrice += (decimal)(item.SingleItemBruttoPrice * (decimal)item.Quantity);
            }

            var payment = await paymentService
                .CreateNewPayment
                ((int)orderModel.PaymentMethodID,
                totalOrderBruttoPrice);

            var customerHistory = await context.CustomerHistory.FirstOrDefaultAsync(x => x.IsActive && x.CustomerID == customer.Id);
            int customerHistoryId = 0;

            if (customerHistory == null)
            {
                customerHistoryId = await customerService.CreateCustomerHistoryAsync(customer.Id);
            }
            else
            {
                customerHistoryId = customerHistory.Id;
            }

            Order order = new Order()
            {
                DeliveryMethodID = orderModel.DeliveryMethodID,
                CustomerID = customer.Id,
                OrderStatusID = 1,
                PaymentID = payment.Id,
                DiscountCodeID = orderModel.DiscountCodeID,
                OrderDate = DateTime.Now,
                CustomerHistoryID = customerHistoryId,
            };

            await context.Order.AddAsync(order);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
            await addressService.AddAddressesForOrderAsync(order.Id, orderAddresses);

            List<OrderItems> orderItems = new List<OrderItems>();
            List<BookItemStockAmountUpdateViewModel> bookItemsForStockUpdate = new List<BookItemStockAmountUpdateViewModel>();
            foreach (var item in orderModel.CartItems)
            {
                orderItems.Add(new OrderItems()
                {
                    BookItemID = item.BookItemID,
                    Quantity = (int)item.Quantity,
                    BruttoPrice = (decimal)item.SingleItemBruttoPrice,
                    TotalBruttoPrice = (decimal)(item.SingleItemBruttoPrice * (decimal)item.Quantity),
                    OrderID = order.Id
                });
                bookItemsForStockUpdate.Add(new BookItemStockAmountUpdateViewModel() { BookItemId = item.BookItemID, Quantity = -(int)item.Quantity });
            }

            await context.OrderItems.AddRangeAsync(orderItems);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);

            await bookItemService.ManageSoldUnitsAsync(orderItems);
            await stockAmountService.UpdateStockAmountAsync(bookItemsForStockUpdate);
        }
        public async Task<InvoiceDataViewModel> GetUserOrderForInvoiceByOrderIdAsync(int orderId)
        {
            var invoice = await context.Order.Where(x => x.IsActive && x.Id == orderId)
                .Select(x => new InvoiceDataViewModel()
                {
                    InvoiceNumber = x.Id,
                    IssueDate = DateTime.Now,
                    DueDate = x.OrderDate,
                    SellerInvoice = new SellerInvoiceViewModel()
                    {
                        Name = "Spellarium",
                        TaxIdentificationNumber = "2814871289823",
                        CityName = "Warszawa",
                        CountryName = "Polska",
                        Postcode = "01-001",
                        Street = "Książkowa",
                        StreetNumber = "47",
                        Email = "spellarium@gmail.com",
                        Phone = "123123123"
                    },
                    CustomerInvoice = new CustomerInvoiceViewModel()
                    {
                        Name = x.CustomerHistory.Name,
                        Surname = x.CustomerHistory.Surname,
                        Email = x.CustomerHistory.Email,
                        Phone = x.CustomerHistory.PhoneNumber,
                        Address = x.OrderAddresses
                        .Where(y => y.IsActive && y.Address.AddressTypeID == 3 && y.OrderID == orderId)
                        .Select(y => new CustomerAddressInvoiceViewModel()
                        {
                            Street = y.Address.Street,
                            StreetNumber = y.Address.StreetNumber,
                            Postcode = y.Address.Postcode,
                            CityName = y.Address.City.Name,
                            CountryName = y.Address.Country.Name,
                            HouseNumber = "/"+y.Address.HouseNumber,
                        }).First(),
                    },
                    AdditionalInfoInvoice = new AdditionalInfoInvoiceViewModel()
                    {
                        PaymentDate = x.Payment.PaymentDate,
                        CurrencyName = "PLN",
                        DeliveryName = x.DeliveryMethod.Name,
                        PaymentMethodName = x.Payment.PaymentMethod.Name,
                    },
                    InvoiceProducts = x.OrderItems
                    .Where(y => y.IsActive)
                    .Select(y => new ProductInvoiceViewModel()
                    {
                        UnitOfMeasure = "szt.",
                        Tax = y.BookItem.Tax,
                        Code = (int)y.BookItemID,
                        Name = y.BookItem.Book.Title,
                        Quantity = y.Quantity,
                        NettoPrice = 0,
                        TaxValue = 0,
                        BruttoPrice = y.BruttoPrice
                    }).ToList(),
                }).FirstOrDefaultAsync();

            foreach(var item in invoice.InvoiceProducts)
            {
                item.NettoPrice = item.BruttoPrice / (decimal)((float)1 + (item.Tax/100));
                item.TaxValue = item.BruttoPrice - item.NettoPrice;
            }

            return invoice;
        }
        public async Task<OrderDiscountCheckViewModel> ApplyDiscountCodeToOrderAsync(OrderDiscountCheckViewModel discountCode)
        {
            var discount = await discountCodeService.CheckIfDiscountCodeIsValidAsync(discountCode.DiscountCode);
            discountCode.CartItems = await SetOriginalPriceForItems(discountCode.CartItems);

            discountCode.CartItems = await bookDiscountService.ApplyDiscount(discountCode.CartItems);
            discountCode.CartItems = await discountCodeService.ApplyDiscountCodeToCartItemsAsync(discountCode.CartItems, discount.Id);
            discountCode.DiscountID = discount.Id;

            return discountCode;
        }
        private async Task<List<OrderItemsListViewModel>> SetOriginalPriceForItems(List<OrderItemsListViewModel> cartItems)
        {
            var bookItems = await bookItemService.GetBookItemsFromOrderAsync(cartItems);
            foreach (var item in cartItems)
            {
                item.SingleItemBruttoPrice = bookItems.Find(x => x.BookItemId == item.BookItemID).BookItemBruttoPrice;
            }
            return cartItems;
        }
    }
}
