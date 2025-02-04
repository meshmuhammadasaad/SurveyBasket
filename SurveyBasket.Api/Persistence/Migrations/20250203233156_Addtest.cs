using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Addtest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "polls:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 2, "permissions", "polls:add", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 3, "permissions", "polls:update", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 4, "permissions", "polls:delete", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 5, "permissions", "questions:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 6, "permissions", "questions:add", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 7, "permissions", "questions:update", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 8, "permissions", "users:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 9, "permissions", "users:add", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 10, "permissions", "users:update", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 11, "permissions", "roles:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 12, "permissions", "roles:add", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 13, "permissions", "roles:update", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 14, "permissions", "results:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" }
                });
        }
    }
}
