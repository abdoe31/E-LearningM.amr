using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlacesWithTimes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacesWithTimes", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlacesWithTimes_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlacesWithTimes_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfflineLectures",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureId = table.Column<int>(type: "int", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: true),
                    QuizGrade = table.Column<int>(type: "int", nullable: true),
                    AssighmentGrade = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceTimeId = table.Column<int>(type: "int", nullable: false),
                    QuizeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfflineLectures", x => x.id);
                    table.ForeignKey(
                        name: "FK_OfflineLectures_Lecture_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lecture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfflineLectures_PlacesWithTimes_PlaceTimeId",
                        column: x => x.PlaceTimeId,
                        principalTable: "PlacesWithTimes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OfflineLectures_Quizes_QuizeId",
                        column: x => x.QuizeId,
                        principalTable: "Quizes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfflineLectures_LectureId",
                table: "OfflineLectures",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineLectures_PlaceTimeId",
                table: "OfflineLectures",
                column: "PlaceTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineLectures_QuizeId",
                table: "OfflineLectures",
                column: "QuizeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacesWithTimes_ClassId",
                table: "PlacesWithTimes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacesWithTimes_PlaceId",
                table: "PlacesWithTimes",
                column: "PlaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfflineLectures");

            migrationBuilder.DropTable(
                name: "PlacesWithTimes");
        }
    }
}
