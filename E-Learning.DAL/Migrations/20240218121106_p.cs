using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class p : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_PlaceWithTimeId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_PlaceWithTimeId",
                table: "User",
                column: "PlaceWithTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_PlaceWithTimeId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_PlaceWithTimeId",
                table: "User",
                column: "PlaceWithTimeId",
                unique: true,
                filter: "[PlaceWithTimeId] IS NOT NULL");
        }
    }
}
