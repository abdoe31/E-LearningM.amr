using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class delteuserlictue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfflineLectures_User_UserId",
                table: "OfflineLectures");

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineLectures_User_UserId",
                table: "OfflineLectures",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfflineLectures_User_UserId",
                table: "OfflineLectures");

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineLectures_User_UserId",
                table: "OfflineLectures",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id");
        }
    }
}
