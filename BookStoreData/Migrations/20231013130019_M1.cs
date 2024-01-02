using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "FooterLinks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "WishlistItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "WishlistItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "UserRecommendedBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "UserRecommendedBooks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Translator",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Translator",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "TransactionsStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "TransactionsStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "SupplyGoods",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "SupplyGoods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Supply",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Supply",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Supplier",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Supplier",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "StockAmount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "StockAmount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "ShippingStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ShippingStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Shipping",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Shipping",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Score",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Reservations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "RentalType",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "RentalType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "RentalStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "RentalStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Rental",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Rental",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "RecommendedBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "RecommendedBooks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Publisher",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Publisher",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Permission",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Permission",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "PaymentMethod",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "PaymentMethod",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Payment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "OrderStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "OrderStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "News",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "News",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "LoginDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "LoginDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Language",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Language",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Images",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Gender",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Gender",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Form",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Form",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "FooterLinks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "FooterLinks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FooterColumnID",
                table: "FooterLinks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "FooterLinks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "FileFormat",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "FileFormat",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Edition",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Edition",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Discount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Discount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "DeliveryStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "DeliveryStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "DeliveryMethod",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "DeliveryMethod",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "CustomerAddress",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "CustomerAddress",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Customer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Country",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Country",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "City",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "City",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Category",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "BookReview",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "BookReview",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "BookItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "BookItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "BookImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "BookImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "BookDiscount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "BookDiscount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "BookCategory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "BookCategory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "BookAuthor",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "BookAuthor",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Book",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "BasketItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "BasketItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Availability",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Availability",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Author",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Author",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Address",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Address",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "AccountStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "AccountStatus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FooterColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<int>(type: "int", nullable: true),
                    HTMLObject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterColumns", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FooterLinks_FooterColumnID",
                table: "FooterLinks",
                column: "FooterColumnID");

            migrationBuilder.AddForeignKey(
                name: "FK_FooterLinks_FooterColumns_FooterColumnID",
                table: "FooterLinks",
                column: "FooterColumnID",
                principalTable: "FooterColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FooterLinks_FooterColumns_FooterColumnID",
                table: "FooterLinks");

            migrationBuilder.DropTable(
                name: "FooterColumns");

            migrationBuilder.DropIndex(
                name: "IX_FooterLinks_FooterColumnID",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "WishlistItem");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "WishlistItem");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "UserRecommendedBooks");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "UserRecommendedBooks");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Translator");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Translator");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "TransactionsStatus");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "TransactionsStatus");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "SupplyGoods");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "SupplyGoods");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Supply");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "StockAmount");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "StockAmount");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "ShippingStatus");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ShippingStatus");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Shipping");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Shipping");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Score");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Score");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "RentalType");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "RentalType");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "RentalStatus");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "RentalStatus");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "RecommendedBooks");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "RecommendedBooks");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Publisher");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Publisher");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Newsletter");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Newsletter");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "News");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "LoginDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "LoginDetails");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Gender");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Gender");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "FooterColumnID",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "URL",
                table: "FooterLinks");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "FileFormat");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "FileFormat");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Edition");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Edition");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "DeliveryMethod");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "DeliveryMethod");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "City");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "City");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "BookReview");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "BookReview");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "BookDiscount");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "BookDiscount");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "BookCategory");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "BookCategory");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "BookAuthor");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "BookAuthor");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Availability");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Availability");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "AccountStatus");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "AccountStatus");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "FooterLinks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FooterLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
