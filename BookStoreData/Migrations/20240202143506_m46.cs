using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class m46 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopBookItemsStatistics");

            migrationBuilder.DropTable(
                name: "TopCategoriesStatistics");

            migrationBuilder.CreateTable(
                name: "BookItemsStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false),
                    SoldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatisticsID = table.Column<int>(type: "int", nullable: false),
                    BookItemID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookItemsStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookItemsStatistics_BookItem_BookItemID",
                        column: x => x.BookItemID,
                        principalTable: "BookItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookItemsStatistics_Statistics_StatisticsID",
                        column: x => x.StatisticsID,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfAppearances = table.Column<int>(type: "int", nullable: false),
                    StatisticsID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesStatistics_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesStatistics_Statistics_StatisticsID",
                        column: x => x.StatisticsID,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookItemsStatistics_BookItemID",
                table: "BookItemsStatistics",
                column: "BookItemID");

            migrationBuilder.CreateIndex(
                name: "IX_BookItemsStatistics_StatisticsID",
                table: "BookItemsStatistics",
                column: "StatisticsID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesStatistics_CategoryID",
                table: "CategoriesStatistics",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesStatistics_StatisticsID",
                table: "CategoriesStatistics",
                column: "StatisticsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookItemsStatistics");

            migrationBuilder.DropTable(
                name: "CategoriesStatistics");

            migrationBuilder.CreateTable(
                name: "TopBookItemsStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookItemID = table.Column<int>(type: "int", nullable: false),
                    StatisticsID = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopBookItemsStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopBookItemsStatistics_BookItem_BookItemID",
                        column: x => x.BookItemID,
                        principalTable: "BookItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopBookItemsStatistics_Statistics_StatisticsID",
                        column: x => x.StatisticsID,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopCategoriesStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    StatisticsID = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopCategoriesStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopCategoriesStatistics_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopCategoriesStatistics_Statistics_StatisticsID",
                        column: x => x.StatisticsID,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopBookItemsStatistics_BookItemID",
                table: "TopBookItemsStatistics",
                column: "BookItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TopBookItemsStatistics_StatisticsID",
                table: "TopBookItemsStatistics",
                column: "StatisticsID");

            migrationBuilder.CreateIndex(
                name: "IX_TopCategoriesStatistics_CategoryID",
                table: "TopCategoriesStatistics",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_TopCategoriesStatistics_StatisticsID",
                table: "TopCategoriesStatistics",
                column: "StatisticsID");
        }
    }
}
