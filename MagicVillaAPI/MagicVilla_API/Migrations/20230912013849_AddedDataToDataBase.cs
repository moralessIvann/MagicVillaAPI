using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataToDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "ActualDate", "Comfort", "CreatedDate", "Detail", "Fee", "ImageUrl", "Name", "Occupants", "SquareMeters" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 11, 18, 38, 48, 998, DateTimeKind.Local).AddTicks(5843), "", new DateTime(2023, 9, 11, 18, 38, 48, 998, DateTimeKind.Local).AddTicks(5800), "Regular", 5.0, "", "Hood Villa", 23, 76 },
                    { 2, new DateTime(2023, 9, 11, 18, 38, 48, 998, DateTimeKind.Local).AddTicks(5849), "", new DateTime(2023, 9, 11, 18, 38, 48, 998, DateTimeKind.Local).AddTicks(5847), "Full", 12.0, "", "Cite Villa", 6, 34 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
