using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m37 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shipping_ShippingID",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingID",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Shipping_ShippingID",
                table: "Order",
                column: "ShippingID",
                principalTable: "Shipping",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shipping_ShippingID",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingID",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Shipping_ShippingID",
                table: "Order",
                column: "ShippingID",
                principalTable: "Shipping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
