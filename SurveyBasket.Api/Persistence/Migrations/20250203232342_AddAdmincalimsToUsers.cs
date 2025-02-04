using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmincalimsToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0194bcd6-15ce-7bb4-9cb1-532a333a007e");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0194bcd6-15ce-7bb4-9cb1-532a333a007e", 0, "3f37231d-5bb0-4bef-b5a5-597a28a3cc20", "Admin@gmail.com", true, "Survey Basket", "Admin", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEFeAMgEPUHsMo2aEADVbF2U8JL7NOSR0Wp2tQDgifirIVBgpb9Ubb0Jzjxan7IUeoA==", null, false, "0EACB59EA2024C938D86995E2A0AEFBF", false, "Admin@gmail.com" });
        }
    }
}
