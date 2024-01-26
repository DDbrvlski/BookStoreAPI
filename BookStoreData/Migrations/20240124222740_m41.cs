using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m41 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supply_PaymentMethod_PaymentMethodID",
                table: "Supply");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodID",
                table: "Supply",
                newName: "PaymentID");

            migrationBuilder.RenameIndex(
                name: "IX_Supply_PaymentMethodID",
                table: "Supply",
                newName: "IX_Supply_PaymentID");

            migrationBuilder.AddColumn<int>(
                name: "BookItemID",
                table: "SupplyGoods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplyID",
                table: "SupplyGoods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyGoods_BookItemID",
                table: "SupplyGoods",
                column: "BookItemID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyGoods_SupplyID",
                table: "SupplyGoods",
                column: "SupplyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Supply_Payment_PaymentID",
                table: "Supply",
                column: "PaymentID",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyGoods_BookItem_BookItemID",
                table: "SupplyGoods",
                column: "BookItemID",
                principalTable: "BookItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyGoods_Supply_SupplyID",
                table: "SupplyGoods",
                column: "SupplyID",
                principalTable: "Supply",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supply_Payment_PaymentID",
                table: "Supply");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyGoods_BookItem_BookItemID",
                table: "SupplyGoods");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyGoods_Supply_SupplyID",
                table: "SupplyGoods");

            migrationBuilder.DropIndex(
                name: "IX_SupplyGoods_BookItemID",
                table: "SupplyGoods");

            migrationBuilder.DropIndex(
                name: "IX_SupplyGoods_SupplyID",
                table: "SupplyGoods");

            migrationBuilder.DropColumn(
                name: "BookItemID",
                table: "SupplyGoods");

            migrationBuilder.DropColumn(
                name: "SupplyID",
                table: "SupplyGoods");

            migrationBuilder.RenameColumn(
                name: "PaymentID",
                table: "Supply",
                newName: "PaymentMethodID");

            migrationBuilder.RenameIndex(
                name: "IX_Supply_PaymentID",
                table: "Supply",
                newName: "IX_Supply_PaymentMethodID");

            migrationBuilder.AddForeignKey(
                name: "FK_Supply_PaymentMethod_PaymentMethodID",
                table: "Supply",
                column: "PaymentMethodID",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
