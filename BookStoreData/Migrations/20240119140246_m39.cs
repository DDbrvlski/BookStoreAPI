using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerHistoryID",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerHistoryID",
                table: "Order",
                column: "CustomerHistoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_CustomerHistory_CustomerHistoryID",
                table: "Order",
                column: "CustomerHistoryID",
                principalTable: "CustomerHistory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_CustomerHistory_CustomerHistoryID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CustomerHistoryID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CustomerHistoryID",
                table: "Order");
        }
    }
}
