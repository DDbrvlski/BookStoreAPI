using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_Edition_EditionID",
                table: "BookItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_FileFormat_FileFormatID",
                table: "BookItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_Translator_TranslatorID",
                table: "BookItem");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSubscribed",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TranslatorID",
                table: "BookItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FileFormatID",
                table: "BookItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EditionID",
                table: "BookItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_Edition_EditionID",
                table: "BookItem",
                column: "EditionID",
                principalTable: "Edition",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_FileFormat_FileFormatID",
                table: "BookItem",
                column: "FileFormatID",
                principalTable: "FileFormat",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_Translator_TranslatorID",
                table: "BookItem",
                column: "TranslatorID",
                principalTable: "Translator",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_Edition_EditionID",
                table: "BookItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_FileFormat_FileFormatID",
                table: "BookItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BookItem_Translator_TranslatorID",
                table: "BookItem");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSubscribed",
                table: "Customer",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "TranslatorID",
                table: "BookItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FileFormatID",
                table: "BookItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EditionID",
                table: "BookItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_Edition_EditionID",
                table: "BookItem",
                column: "EditionID",
                principalTable: "Edition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_FileFormat_FileFormatID",
                table: "BookItem",
                column: "FileFormatID",
                principalTable: "FileFormat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookItem_Translator_TranslatorID",
                table: "BookItem",
                column: "TranslatorID",
                principalTable: "Translator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
