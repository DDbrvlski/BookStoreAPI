using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class M9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionStatus",
                table: "Payment");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_TransactionStatusID",
                table: "Payment",
                column: "TransactionStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_TransactionsStatus_TransactionStatusID",
                table: "Payment",
                column: "TransactionStatusID",
                principalTable: "TransactionsStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_TransactionsStatus_TransactionStatusID",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_TransactionStatusID",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "TransactionStatus",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
