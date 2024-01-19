using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m38 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerHistoryId",
                table: "CustomerAddress",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerHistory_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CustomerHistoryId",
                table: "CustomerAddress",
                column: "CustomerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerHistory_CustomerID",
                table: "CustomerHistory",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAddress_CustomerHistory_CustomerHistoryId",
                table: "CustomerAddress",
                column: "CustomerHistoryId",
                principalTable: "CustomerHistory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAddress_CustomerHistory_CustomerHistoryId",
                table: "CustomerAddress");

            migrationBuilder.DropTable(
                name: "CustomerHistory");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAddress_CustomerHistoryId",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "CustomerHistoryId",
                table: "CustomerAddress");
        }
    }
}
