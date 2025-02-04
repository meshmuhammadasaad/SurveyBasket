using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0739f434-25b7-44da-88a9-bfe5b75ed5e5", "44ee415f-b399-4f80-9d3d-ccf4a789b470", false, false, "Admin", "ADMIN" },
                    { "2e373773-3cdc-4e6f-859b-b918cee60368", "6309d2e2-087f-4e11-bc5e-9d81480706d9", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0194bcd6-15ce-7bb4-9cb1-532a333a007e", 0, "3f37231d-5bb0-4bef-b5a5-597a28a3cc20", "Admin@gmail.com", true, "Survey Basket", "Admin", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEFeAMgEPUHsMo2aEADVbF2U8JL7NOSR0Wp2tQDgifirIVBgpb9Ubb0Jzjxan7IUeoA==", null, false, "0EACB59EA2024C938D86995E2A0AEFBF", false, "Admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissins", "polls:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 2, "permissins", "polls:add", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 3, "permissins", "polls:update", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 4, "permissins", "polls:delete", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 5, "permissins", "questions:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 6, "permissins", "questions:add", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 7, "permissins", "questions:update", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 8, "permissins", "users:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 9, "permissins", "users:add", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 10, "permissins", "users:update", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 11, "permissins", "roles:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 12, "permissins", "roles:add", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 13, "permissins", "roles:update", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" },
                    { 14, "permissins", "results:read", "0739f434-25b7-44da-88a9-bfe5b75ed5e5" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0739f434-25b7-44da-88a9-bfe5b75ed5e5", "0194bcd6-15ce-7bb4-9cb1-532a333a007e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e373773-3cdc-4e6f-859b-b918cee60368");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0739f434-25b7-44da-88a9-bfe5b75ed5e5", "0194bcd6-15ce-7bb4-9cb1-532a333a007e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0739f434-25b7-44da-88a9-bfe5b75ed5e5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0194bcd6-15ce-7bb4-9cb1-532a333a007e");
        }
    }
}
