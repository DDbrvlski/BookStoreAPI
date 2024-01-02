using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Language_LanguageID",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_Translator_TranslatorID",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_LanguageID",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_TranslatorID",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ISBN10",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ISBN13",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "LanguageID",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Pages",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "PublishingDate",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "TranslatorID",
                table: "Book");

            migrationBuilder.AddColumn<int>(
                name: "AccountStatusID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImageID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoginDetailsID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PermissionID",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryMethodID",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OrderStatusID",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "BookItem",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LanguageID",
                table: "BookItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pages",
                table: "BookItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishingDate",
                table: "BookItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TranslatorID",
                table: "BookItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ListOfIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    BookId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListOfIds_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListOfIds_Book_BookId1",
                        column: x => x.BookId1,
                        principalTable: "Book",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountStatusID",
                table: "User",
                column: "AccountStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_User_CustomerID",
                table: "User",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_User_ImageID",
                table: "User",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_User_LoginDetailsID",
                table: "User",
                column: "LoginDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_User_PermissionID",
                table: "User",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliveryMethodID",
                table: "Order",
                column: "DeliveryMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderStatusID",
                table: "Order",
                column: "OrderStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_BookItem_LanguageID",
                table: "BookItem",
                column: "LanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_BookItem_TranslatorID",
                table: "BookItem",
                column: "TranslatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfIds_BookId",
                table: "ListOfIds",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfIds_BookId1",
                table: "ListOfIds",
                column: "BookId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_Language_LanguageID",
                table: "BookItem",
                column: "LanguageID",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_Translator_TranslatorID",
                table: "BookItem",
                column: "TranslatorID",
                principalTable: "Translator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_DeliveryMethod_DeliveryMethodID",
                table: "Order",
                column: "DeliveryMethodID",
                principalTable: "DeliveryMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderStatus_OrderStatusID",
                table: "Order",
                column: "OrderStatusID",
                principalTable: "OrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_AccountStatus_AccountStatusID",
                table: "User",
                column: "AccountStatusID",
                principalTable: "AccountStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Customer_CustomerID",
                table: "User",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Images_ImageID",
                table: "User",
                column: "ImageID",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_LoginDetails_LoginDetailsID",
                table: "User",
                column: "LoginDetailsID",
                principalTable: "LoginDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Permission_PermissionID",
                table: "User",
                column: "PermissionID",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_Language_LanguageID",
                table: "BookItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_Translator_TranslatorID",
                table: "BookItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_DeliveryMethod_DeliveryMethodID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderStatus_OrderStatusID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_User_AccountStatus_AccountStatusID",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Customer_CustomerID",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Images_ImageID",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_LoginDetails_LoginDetailsID",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Permission_PermissionID",
                table: "User");

            migrationBuilder.DropTable(
                name: "ListOfIds");

            migrationBuilder.DropIndex(
                name: "IX_User_AccountStatusID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CustomerID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ImageID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_LoginDetailsID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PermissionID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Order_DeliveryMethodID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderStatusID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_BookItem_LanguageID",
                table: "BookItem");

            migrationBuilder.DropIndex(
                name: "IX_BookItem_TranslatorID",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "AccountStatusID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LoginDetailsID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PermissionID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeliveryMethodID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderStatusID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "LanguageID",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "Pages",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "PublishingDate",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "TranslatorID",
                table: "BookItem");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISBN10",
                table: "Book",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISBN13",
                table: "Book",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LanguageID",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pages",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishingDate",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TranslatorID",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Book_LanguageID",
                table: "Book",
                column: "LanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_Book_TranslatorID",
                table: "Book",
                column: "TranslatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Language_LanguageID",
                table: "Book",
                column: "LanguageID",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Translator_TranslatorID",
                table: "Book",
                column: "TranslatorID",
                principalTable: "Translator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
