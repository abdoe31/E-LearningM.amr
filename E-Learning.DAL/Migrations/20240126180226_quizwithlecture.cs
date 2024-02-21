using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class quizwithlecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizType",
                table: "UserQuiz",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AcessType",
                table: "UserLecture",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LectureType",
                table: "UserLecture",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuizType",
                table: "UserQuiz");

            migrationBuilder.DropColumn(
                name: "LectureType",
                table: "UserLecture");

            migrationBuilder.AlterColumn<int>(
                name: "AcessType",
                table: "UserLecture",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
