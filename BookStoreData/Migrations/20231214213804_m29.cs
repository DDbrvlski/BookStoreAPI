using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_DiscountCode_DiscountCodeId",
                table: "BookItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_AspNetUsers_UserID",
                table: "Rental");

            migrationBuilder.DropTable(
                name: "BookDiscountCode");

            migrationBuilder.DropIndex(
                name: "IX_Rental_UserID",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_BookItem_DiscountCodeId",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "BookItem");

            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "Rental",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodID",
                table: "Rental",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeID",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rental_CustomerID",
                table: "Rental",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_PaymentMethodID",
                table: "Rental",
                column: "PaymentMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DiscountCodeID",
                table: "Order",
                column: "DiscountCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_DiscountCode_DiscountCodeID",
                table: "Order",
                column: "DiscountCodeID",
                principalTable: "DiscountCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Customer_CustomerID",
                table: "Rental",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_PaymentMethod_PaymentMethodID",
                table: "Rental",
                column: "PaymentMethodID",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_DiscountCode_DiscountCodeID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Customer_CustomerID",
                table: "Rental");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_PaymentMethod_PaymentMethodID",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_CustomerID",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_PaymentMethodID",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Order_DiscountCodeID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "PaymentMethodID",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "DiscountCodeID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Rental",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeId",
                table: "BookItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookDiscountCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookItemID = table.Column<int>(type: "int", nullable: false),
                    DiscountCodeID = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDiscountCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookDiscountCode_BookItem_BookItemID",
                        column: x => x.BookItemID,
                        principalTable: "BookItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookDiscountCode_DiscountCode_DiscountCodeID",
                        column: x => x.DiscountCodeID,
                        principalTable: "DiscountCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rental_UserID",
                table: "Rental",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_BookItem_DiscountCodeId",
                table: "BookItem",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookDiscountCode_BookItemID",
                table: "BookDiscountCode",
                column: "BookItemID");

            migrationBuilder.CreateIndex(
                name: "IX_BookDiscountCode_DiscountCodeID",
                table: "BookDiscountCode",
                column: "DiscountCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_DiscountCode_DiscountCodeId",
                table: "BookItem",
                column: "DiscountCodeId",
                principalTable: "DiscountCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_AspNetUsers_UserID",
                table: "Rental",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
