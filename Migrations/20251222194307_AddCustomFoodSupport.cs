using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomFoodSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCustom",
                table: "FoodLibraries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FoodLibraries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });

            migrationBuilder.UpdateData(
                table: "FoodLibraries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "IsCustom", "UserId" },
                values: new object[] { false, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCustom",
                table: "FoodLibraries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FoodLibraries");
        }
    }
}
