using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReview_User_UserID",
                table: "BookReview");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_User_UserID",
                table: "Rental");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_User_UserID",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRecommendedBooks_User_UserID",
                table: "UserRecommendedBooks");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "LoginDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserRecommendedBooks_UserID",
                table: "UserRecommendedBooks");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Rental_UserID",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_BookReview_UserID",
                table: "BookReview");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "UserRecommendedBooks");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "BookReview");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "UserRecommendedBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Rental",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "BookReview",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LoginDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountStatusID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    ImageID = table.Column<int>(type: "int", nullable: false),
                    LoginDetailsID = table.Column<int>(type: "int", nullable: false),
                    PermissionID = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_AccountStatus_AccountStatusID",
                        column: x => x.AccountStatusID,
                        principalTable: "AccountStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Images_ImageID",
                        column: x => x.ImageID,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_LoginDetails_LoginDetailsID",
                        column: x => x.LoginDetailsID,
                        principalTable: "LoginDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Permission_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRecommendedBooks_UserID",
                table: "UserRecommendedBooks",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserID",
                table: "Reservations",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_UserID",
                table: "Rental",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_BookReview_UserID",
                table: "BookReview",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountStatusID",
                table: "User",
                column: "AccountStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_User_CustomerID",
                table: "User",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_User_ImageID",
                table: "User",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_User_LoginDetailsID",
                table: "User",
                column: "LoginDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_User_PermissionID",
                table: "User",
                column: "PermissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReview_User_UserID",
                table: "BookReview",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_User_UserID",
                table: "Rental",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_User_UserID",
                table: "Reservations",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRecommendedBooks_User_UserID",
                table: "UserRecommendedBooks",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
