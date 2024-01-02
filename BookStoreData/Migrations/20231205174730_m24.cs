using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "CategoryElement",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryElement_CategoryID",
                table: "CategoryElement",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryElement_Category_CategoryID",
                table: "CategoryElement",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryElement_Category_CategoryID",
                table: "CategoryElement");

            migrationBuilder.DropIndex(
                name: "IX_CategoryElement_CategoryID",
                table: "CategoryElement");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "CategoryElement");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
