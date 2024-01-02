using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_AddressType_AddressTypeID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_FooterLinks_FooterColumns_FooterColumnID",
                table: "FooterLinks");

            migrationBuilder.DropTable(
                name: "AddressType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FooterColumns",
                table: "FooterColumns");

            migrationBuilder.DropIndex(
                name: "IX_Address_AddressTypeID",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "FooterColumns",
                newName: "footerColumns");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Banner",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "AddressTypeID",
                table: "Address",
                newName: "Position");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Newsletter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSent",
                table: "Newsletter",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Newsletter",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_footerColumns",
                table: "footerColumns",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountsBanner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountsBanner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountsBanner_Images_ImageID",
                        column: x => x.ImageID,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NewsletterSubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsletterSubscribers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountsBanner_ImageID",
                table: "DiscountsBanner",
                column: "ImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_FooterLinks_footerColumns_FooterColumnID",
                table: "FooterLinks",
                column: "FooterColumnID",
                principalTable: "footerColumns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FooterLinks_footerColumns_FooterColumnID",
                table: "FooterLinks");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "DiscountsBanner");

            migrationBuilder.DropTable(
                name: "NewsletterSubscribers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_footerColumns",
                table: "footerColumns");

            migrationBuilder.RenameTable(
                name: "footerColumns",
                newName: "FooterColumns");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Banner",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Address",
                newName: "AddressTypeID");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Newsletter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Newsletter",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSent",
                table: "Newsletter",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Newsletter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FooterColumns",
                table: "FooterColumns",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AddressType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressType", x => x.Id);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_FooterLinks_FooterColumns_FooterColumnID",
                table: "FooterLinks",
                column: "FooterColumnID",
                principalTable: "FooterColumns",
                principalColumn: "Id");
        }
    }
}
