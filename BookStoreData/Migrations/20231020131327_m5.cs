using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valid",
                table: "Discount");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageURL",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeId",
                table: "BookItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "BookItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PercentOfDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookDiscountCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookItemID = table.Column<int>(type: "int", nullable: false),
                    DiscountCodeID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDiscountCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookDiscountCode_BookItem_BookItemID",
                        column: x => x.BookItemID,
                        principalTable: "BookItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookDiscountCode_DiscountCode_DiscountCodeID",
                        column: x => x.DiscountCodeID,
                        principalTable: "DiscountCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookItem_DiscountCodeId",
                table: "BookItem",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookItem_DiscountId",
                table: "BookItem",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_BookDiscountCode_BookItemID",
                table: "BookDiscountCode",
                column: "BookItemID");

            migrationBuilder.CreateIndex(
                name: "IX_BookDiscountCode_DiscountCodeID",
                table: "BookDiscountCode",
                column: "DiscountCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_DiscountCode_DiscountCodeId",
                table: "BookItem",
                column: "DiscountCodeId",
                principalTable: "DiscountCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_Discount_DiscountId",
                table: "BookItem",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_DiscountCode_DiscountCodeId",
                table: "BookItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_Discount_DiscountId",
                table: "BookItem");

            migrationBuilder.DropTable(
                name: "BookDiscountCode");

            migrationBuilder.DropTable(
                name: "DiscountCode");

            migrationBuilder.DropIndex(
                name: "IX_BookItem_DiscountCodeId",
                table: "BookItem");

            migrationBuilder.DropIndex(
                name: "IX_BookItem_DiscountId",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "BookItem");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "BookItem");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageURL",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Valid",
                table: "Discount",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
