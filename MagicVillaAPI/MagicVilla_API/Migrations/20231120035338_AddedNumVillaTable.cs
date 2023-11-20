using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AddedNumVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Detail",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Comfort",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "VillaNum",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaNum", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_VillaNum_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActualDate", "CreatedDate" },
                values: new object[] { new DateTime(2023, 11, 19, 19, 53, 38, 11, DateTimeKind.Local).AddTicks(9019), new DateTime(2023, 11, 19, 19, 53, 38, 11, DateTimeKind.Local).AddTicks(8933) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ActualDate", "CreatedDate" },
                values: new object[] { new DateTime(2023, 11, 19, 19, 53, 38, 11, DateTimeKind.Local).AddTicks(9031), new DateTime(2023, 11, 19, 19, 53, 38, 11, DateTimeKind.Local).AddTicks(9029) });

            migrationBuilder.CreateIndex(
                name: "IX_VillaNum_VillaId",
                table: "VillaNum",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaNum");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Detail",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comfort",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActualDate", "CreatedDate" },
                values: new object[] { new DateTime(2023, 9, 11, 18, 38, 48, 998, DateTimeKind.Local).AddTicks(5843), new DateTime(2023, 9, 11, 18, 38, 48, 998, DateTimeKind.Local).AddTicks(5800) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ActualDate", "CreatedDate" },
                values: new object[] { new DateTime(2023, 9, 11, 18, 38, 48, 998, DateTimeKind.Local).AddTicks(5849), new DateTime(2023, 9, 11, 18, 38, 48, 998, DateTimeKind.Local).AddTicks(5847) });
        }
    }
}
