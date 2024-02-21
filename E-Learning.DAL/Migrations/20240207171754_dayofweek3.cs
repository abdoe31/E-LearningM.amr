using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class dayofweek3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfflineLectures_Lecture_LectureId",
                table: "OfflineLectures");

            migrationBuilder.DropForeignKey(
                name: "FK_OfflineLectures_PlacesWithTimes_PlaceTimeId",
                table: "OfflineLectures");

            migrationBuilder.DropForeignKey(
                name: "FK_OfflineLectures_User_UserId",
                table: "OfflineLectures");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "OfflineLectures",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceTimeId",
                table: "OfflineLectures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LectureId",
                table: "OfflineLectures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Attend",
                table: "OfflineLectures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "OfflineLectures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "OfflineLectures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineLectures_Lecture_LectureId",
                table: "OfflineLectures",
                column: "LectureId",
                principalTable: "Lecture",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineLectures_PlacesWithTimes_PlaceTimeId",
                table: "OfflineLectures",
                column: "PlaceTimeId",
                principalTable: "PlacesWithTimes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineLectures_User_UserId",
                table: "OfflineLectures",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfflineLectures_Lecture_LectureId",
                table: "OfflineLectures");

            migrationBuilder.DropForeignKey(
                name: "FK_OfflineLectures_PlacesWithTimes_PlaceTimeId",
                table: "OfflineLectures");

            migrationBuilder.DropForeignKey(
                name: "FK_OfflineLectures_User_UserId",
                table: "OfflineLectures");

            migrationBuilder.DropColumn(
                name: "Attend",
                table: "OfflineLectures");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "OfflineLectures");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "OfflineLectures");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "OfflineLectures",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlaceTimeId",
                table: "OfflineLectures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LectureId",
                table: "OfflineLectures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineLectures_Lecture_LectureId",
                table: "OfflineLectures",
                column: "LectureId",
                principalTable: "Lecture",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineLectures_PlacesWithTimes_PlaceTimeId",
                table: "OfflineLectures",
                column: "PlaceTimeId",
                principalTable: "PlacesWithTimes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineLectures_User_UserId",
                table: "OfflineLectures",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
