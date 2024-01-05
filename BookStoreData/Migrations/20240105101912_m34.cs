using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Address_CustomerID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "Order");

            migrationBuilder.CreateTable(
                name: "OrderAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    AddressID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderAddress_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderAddress_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderAddress_AddressID",
                table: "OrderAddress",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAddress_OrderID",
                table: "OrderAddress",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderAddress");

            migrationBuilder.AddColumn<int>(
                name: "AddressID",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Address_CustomerID",
                table: "Order",
                column: "CustomerID",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
