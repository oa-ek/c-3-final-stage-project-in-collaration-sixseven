using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZNOWay.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalQuestionsToUserResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "UserResults",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "UserResults");
        }
    }
}
