using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyGoods_BookItem_BookItemID",
                table: "SupplyGoods");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyGoods_Supply_SupplyID",
                table: "SupplyGoods");

            migrationBuilder.AlterColumn<int>(
                name: "SupplyID",
                table: "SupplyGoods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookItemID",
                table: "SupplyGoods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "SupplyGoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "Supply",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyGoods_BookItem_BookItemID",
                table: "SupplyGoods",
                column: "BookItemID",
                principalTable: "BookItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyGoods_Supply_SupplyID",
                table: "SupplyGoods",
                column: "SupplyID",
                principalTable: "Supply",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyGoods_BookItem_BookItemID",
                table: "SupplyGoods");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyGoods_Supply_SupplyID",
                table: "SupplyGoods");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SupplyGoods");

            migrationBuilder.AlterColumn<int>(
                name: "SupplyID",
                table: "SupplyGoods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BookItemID",
                table: "SupplyGoods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "Supply",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
    }
}
