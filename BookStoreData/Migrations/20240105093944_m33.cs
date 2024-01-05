using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressTypeID",
                table: "Address",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_AddressTypeID",
                table: "Address",
                column: "AddressTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_AddressType_AddressTypeID",
                table: "Address",
                column: "AddressTypeID",
                principalTable: "AddressType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_AddressType_AddressTypeID",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_AddressTypeID",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "AddressTypeID",
                table: "Address");
        }
    }
}
