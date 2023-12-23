using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class final1512 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Lectureid",
                table: "LectureCode",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Classid",
                table: "LectureCode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CodeTybe",
                table: "LectureCode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LectureCode_Classid",
                table: "LectureCode",
                column: "Classid");

            migrationBuilder.AddForeignKey(
                name: "FK_LectureCode_Classes_Classid",
                table: "LectureCode",
                column: "Classid",
                principalTable: "Classes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LectureCode_Classes_Classid",
                table: "LectureCode");

            migrationBuilder.DropIndex(
                name: "IX_LectureCode_Classid",
                table: "LectureCode");

            migrationBuilder.DropColumn(
                name: "Classid",
                table: "LectureCode");

            migrationBuilder.DropColumn(
                name: "CodeTybe",
                table: "LectureCode");

            migrationBuilder.AlterColumn<int>(
                name: "Lectureid",
                table: "LectureCode",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
