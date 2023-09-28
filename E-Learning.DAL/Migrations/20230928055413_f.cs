using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class f : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Videofiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Leactureid = table.Column<int>(type: "int", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number = table.Column<int>(type: "int", nullable: true),
                    PartHeader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videofiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videofiles_Lecture_Leactureid",
                        column: x => x.Leactureid,
                        principalTable: "Lecture",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videofiles_Leactureid",
                table: "Videofiles",
                column: "Leactureid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Videofiles");
        }
    }
}
