using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetupWithData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "Electronics", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Laptop", 1200.00m, 10 },
                    { 2, "Electronics", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mouse", 25.50m, 50 },
                    { 3, "Furniture", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Office Chair", 150.00m, 15 },
                    { 4, "Kitchen", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Coffee Mug", 12.99m, 100 },
                    { 5, "Stationery", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Notebook", 5.00m, 200 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
