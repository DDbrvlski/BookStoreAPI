using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Address");

            migrationBuilder.AddColumn<int>(
                name: "AddressID",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AddressType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressType", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Address_CustomerID",
                table: "Order",
                column: "CustomerID",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Address_CustomerID",
                table: "Order");

            migrationBuilder.DropTable(
                name: "AddressType");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Address",
                type: "int",
                nullable: true);
        }
    }
}
