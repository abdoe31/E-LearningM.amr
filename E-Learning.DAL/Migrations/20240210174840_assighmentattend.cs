using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class assighmentattend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AssighmentAttend",
                table: "OfflineLectures",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssighmentAttend",
                table: "OfflineLectures");
        }
    }
}
