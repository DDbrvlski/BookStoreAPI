using BookStoreData.Models.Accounts;
using BookStoreData.Models.Basket;
using BookStoreData.Models.Customers;
using BookStoreData.Models.Customers.AddressDictionaries;
using BookStoreData.Models.Delivery;
using BookStoreData.Models.Delivery.Dictionaries;
using BookStoreData.Models.Media;
using BookStoreData.Models.Notifications;
using BookStoreData.Models.Orders;
using BookStoreData.Models.Orders.Dictionaries;
using BookStoreData.Models.PageContent;
using BookStoreData.Models.Products.BookItems;
using BookStoreData.Models.Products.BookItems.BookItemDictionaries;
using BookStoreData.Models.Products.Books;
using BookStoreData.Models.Products.Books.BookDictionaries;
using BookStoreData.Models.Rental;
using BookStoreData.Models.Rentals.Dictionaries;
using BookStoreData.Models.Supplies;
using BookStoreData.Models.Supplies.Dictionaries;
using BookStoreData.Models.Supply;
using BookStoreData.Models.Transactions;
using BookStoreData.Models.Transactions.Dictionaries;
using BookStoreData.Models.Wishlist;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreData.Data
{
    public class BookStoreContext : IdentityDbContext<User>
    {
        //private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }


        //Accounts
        public DbSet<User> User { get; set; }

        //Basket
        public DbSet<BasketItem> BasketItem { get; set; }

        //Customers
        public DbSet<City> City { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerAddress> CustomerAddress { get; set; }

        //Delivery
        public DbSet<ShippingStatus> ShippingStatus { get; set; }
        public DbSet<Shipping> Shipping { get; set; }

        //Media
        public DbSet<Images> Images { get; set; }

        //Notifications
        public DbSet<Newsletter> Newsletter { get; set; }
        public DbSet<NewsletterSubscribers> NewsletterSubscribers { get; set; }
        public DbSet<Contact> Contact { get; set; }

        //Orders
        public DbSet<DeliveryMethod> DeliveryMethod { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Order> Order { get; set; }

        //PageContent
        public DbSet<Banner> Banner { get; set; }
        public DbSet<CategoryElement> CategoryElement { get; set; }
        public DbSet<DiscountsBanner> DiscountsBanner { get; set; }
        public DbSet<FooterLinks> FooterLinks { get; set; }
        public DbSet<FooterColumns> footerColumns { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NavBarMenuLinks> NavBarMenuLinks { get; set; }

        //Products
        //BookItems
        public DbSet<Availability> Availability { get; set; }
        public DbSet<Edition> Edition { get; set; }
        public DbSet<FileFormat> FileFormat { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<BookDiscount> BookDiscount { get; set; }
        public DbSet<BookItem> BookItem { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<DiscountCode> DiscountCode { get; set; }
        public DbSet<RecommendedBooks> RecommendedBooks { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<StockAmount> StockAmount { get; set; }
        public DbSet<UserRecommendedBooks> UserRecommendedBooks { get; set; }
        public DbSet<BookItemReview> BookItemReview { get; set; }
        //Books
        public DbSet<Author> Author { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Score> Score { get; set; }
        public DbSet<Translator> Translator { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<BookCategory> BookCategory { get; set; }
        public DbSet<BookImages> BookImages { get; set; }

        //Rentals
        public DbSet<RentalStatus> RentalStatus { get; set; }
        public DbSet<RentalType> RentalType { get; set; }
        public DbSet<Rental> Rental { get; set; }

        //Supplies
        public DbSet<DeliveryStatus> DeliveryStatus { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Supply> Supply { get; set; }
        public DbSet<SupplyGoods> SupplyGoods { get; set; }

        //Transactions
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<TransactionsStatus> TransactionsStatus { get; set; }
        public DbSet<Payment> Payment { get; set; }

        //Wishlist
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<WishlistItems> WishlistItems { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //    .UseLoggerFactory(MyLoggerFactory); // Dodaj to
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookItem>()
                .HasOne(b => b.Language)
                .WithMany()
                .HasForeignKey(b => b.LanguageID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.OriginalLanguage)
                .WithMany()
                .HasForeignKey(b => b.OriginalLanguageID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .Property(u => u.CustomerID)
                .IsRequired(false);

            modelBuilder.Entity<BookItem>()
                .Property(u => u.TranslatorID)
                .IsRequired(false);
            modelBuilder.Entity<BookItem>()
                .Property(u => u.EditionID)
                .IsRequired(false);
            modelBuilder.Entity<BookItem>()
                .Property(u => u.LanguageID)
                .IsRequired(false);
            modelBuilder.Entity<BookItem>()
                .Property(u => u.FileFormatID)
                .IsRequired(false);
        }
    }
}
