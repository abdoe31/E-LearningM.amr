using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fffffF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OfflineLectures",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineLectures_UserId",
                table: "OfflineLectures",
                column: "UserId");

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

            migrationBuilder.DropIndex(
                name: "IX_OfflineLectures_UserId",
                table: "OfflineLectures");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OfflineLectures");
        }
    }
}
