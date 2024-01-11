using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class parent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ParentId",
                table: "User",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_ParentId",
                table: "User",
                column: "ParentId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_ParentId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ParentId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "User");
        }
    }
}
