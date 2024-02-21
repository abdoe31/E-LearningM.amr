using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class gradesandplace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssighmentGrade",
                table: "UserLecture",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlaceWithTimeId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PlaceWithTimeId",
                table: "User",
                column: "PlaceWithTimeId",
                unique: true,
                filter: "[PlaceWithTimeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_User_PlacesWithTimes_PlaceWithTimeId",
                table: "User",
                column: "PlaceWithTimeId",
                principalTable: "PlacesWithTimes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_PlacesWithTimes_PlaceWithTimeId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PlaceWithTimeId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AssighmentGrade",
                table: "UserLecture");

            migrationBuilder.DropColumn(
                name: "PlaceWithTimeId",
                table: "User");
        }
    }
}
