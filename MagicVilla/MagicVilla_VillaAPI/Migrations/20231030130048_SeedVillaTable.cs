using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2023, 10, 30, 16, 0, 48, 606, DateTimeKind.Local).AddTicks(2898), "Royal Villa (1 smal room, 1 big room for 4 people(ocupance);", "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa3.jpg", "Royal Villa", 4, 200.0, 550, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "", new DateTime(2023, 10, 30, 16, 0, 48, 606, DateTimeKind.Local).AddTicks(2912), "Premium Pool Villa (2 big rooms for 4 people(ocupance) && 1 small  pool;", "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa1.jpg", "Premium Pool Villa", 4, 300.0, 550, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "", new DateTime(2023, 10, 30, 16, 0, 48, 606, DateTimeKind.Local).AddTicks(2914), "Luxary Pool Villa (2 big rooms for 4 people(ocupance) && 1 big pool;", "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa4.jpg", "Luxary Pool Villa", 4, 400.0, 750, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "", new DateTime(2023, 10, 30, 16, 0, 48, 606, DateTimeKind.Local).AddTicks(2916), "Diamond Pool Villa (2 very  big rooms for 4 people(ocupance);", "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa2.jpg", "Diamond Villa", 4, 550.0, 750, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "", new DateTime(2023, 10, 30, 16, 0, 48, 606, DateTimeKind.Local).AddTicks(2917), "Diamond Pool Villa (2 very big rooms for 4 people(ocupance) && 2 big pool;", "https://dotnetmasteryimages.blob.core.windows.net/blueVillaimages/villa5.jpg", "Diamond Pool Villa", 4, 600.0, 1100, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
