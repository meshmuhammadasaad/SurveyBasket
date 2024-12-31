using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditonAnswersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestianId_Content",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers");

            //migrationBuilder.DropColumn(
            //    name: "QuestianId",
            //    table: "Answers");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId_Content",
                table: "Answers",
                columns: new[] { "QuestionId", "Content" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId_Content",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "QuestianId",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestianId_Content",
                table: "Answers",
                columns: new[] { "QuestianId", "Content" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");
        }
    }
}
