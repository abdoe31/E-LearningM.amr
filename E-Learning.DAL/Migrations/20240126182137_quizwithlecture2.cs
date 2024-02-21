using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class quizwithlecture2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "UserQuiz",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "UserLecture",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuiz_PlaceId",
                table: "UserQuiz",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLecture_PlaceId",
                table: "UserLecture",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLecture_Places_PlaceId",
                table: "UserLecture",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuiz_Places_PlaceId",
                table: "UserQuiz",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLecture_Places_PlaceId",
                table: "UserLecture");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuiz_Places_PlaceId",
                table: "UserQuiz");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropIndex(
                name: "IX_UserQuiz_PlaceId",
                table: "UserQuiz");

            migrationBuilder.DropIndex(
                name: "IX_UserLecture_PlaceId",
                table: "UserLecture");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "UserQuiz");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "UserLecture");
        }
    }
}
