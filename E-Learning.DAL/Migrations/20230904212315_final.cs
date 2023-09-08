using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assighment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelAnswerFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Classid = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assighment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Quizes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    header = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Classid = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Year",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "(N'')"),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Year", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    yearid = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Classes_Year",
                        column: x => x.yearid,
                        principalTable: "Year",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SecondName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentPhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "bit", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Yearid = table.Column<int>(type: "int", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pasword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Year_Yearid",
                        column: x => x.Yearid,
                        principalTable: "Year",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Lecture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Classid = table.Column<int>(type: "int", nullable: true),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quizid = table.Column<int>(type: "int", nullable: true),
                    number = table.Column<int>(type: "int", nullable: true),
                    Assighnmentid = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecture", x => x.id);
                    table.ForeignKey(
                        name: "FK_Lecture_Assighment",
                        column: x => x.Assighnmentid,
                        principalTable: "Assighment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Lecture_Classes",
                        column: x => x.Classid,
                        principalTable: "Classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lecture_Quizes",
                        column: x => x.Quizid,
                        principalTable: "Quizes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Classid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notifications_Classes_Classid",
                        column: x => x.Classid,
                        principalTable: "Classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAssighment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Studentid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Assighmentid = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    solved = table.Column<bool>(type: "bit", nullable: true),
                    @checked = table.Column<bool>(name: "checked", type: "bit", nullable: true),
                    UserAnswerFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssighment", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserAssighment_Assighment",
                        column: x => x.Assighmentid,
                        principalTable: "Assighment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAssighment_User",
                        column: x => x.Studentid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClasses",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClasses", x => new { x.UserId, x.ClassId });
                    table.ForeignKey(
                        name: "FK_UserClasses_Classes",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClasses_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClassRequists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Userid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Classid = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClassRequists", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserClassRequists_Classes_Classid",
                        column: x => x.Classid,
                        principalTable: "Classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UserClassRequists_User_Userid",
                        column: x => x.Userid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuiz",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Studentid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Quizid = table.Column<int>(type: "int", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime", nullable: true),
                    End = table.Column<DateTime>(type: "datetime", nullable: true),
                    Grade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuiz", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserQuiz_Quizes",
                        column: x => x.Quizid,
                        principalTable: "Quizes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuiz_User",
                        column: x => x.Studentid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureCode",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lectureid = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GeneratedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    Usedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    used = table.Column<bool>(type: "bit", nullable: false),
                    QuizRequired = table.Column<bool>(type: "bit", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureCode", x => x.id);
                    table.ForeignKey(
                        name: "FK_LectureCode_Lecture",
                        column: x => x.Lectureid,
                        principalTable: "Lecture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LectureCode_User",
                        column: x => x.StudentId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLecture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Lectureid = table.Column<int>(type: "int", nullable: true),
                    AcessType = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime", nullable: true),
                    end = table.Column<DateTime>(type: "datetime", nullable: true),
                    QuizRequired = table.Column<bool>(type: "bit", nullable: false),
                    QuizSolved = table.Column<bool>(type: "bit", nullable: false),
                    AssighmentSolved = table.Column<bool>(type: "bit", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLecture", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserLecture_Lecture",
                        column: x => x.Lectureid,
                        principalTable: "Lecture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLecture_User",
                        column: x => x.StudentId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "videoParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Leactureid = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number = table.Column<int>(type: "int", nullable: true),
                    PartHeader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "(N'')"),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videoParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_videoParts_Lecture",
                        column: x => x.Leactureid,
                        principalTable: "Lecture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Questionid = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightAnswerid = table.Column<int>(type: "int", nullable: true),
                    QuizId = table.Column<int>(type: "int", nullable: true),
                    Answerid = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Questions_Answers_RightAnswerid",
                        column: x => x.RightAnswerid,
                        principalTable: "Answers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Questions_Quizes",
                        column: x => x.QuizId,
                        principalTable: "Quizes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    UserQuizId = table.Column<int>(type: "int", nullable: true),
                    right = table.Column<bool>(type: "bit", nullable: true),
                    Answerid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswer", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserAnswer_Answers_Answerid",
                        column: x => x.Answerid,
                        principalTable: "Answers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UserAnswer_Questions",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UserAnswer_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UserAnswer_UserQuiz",
                        column: x => x.UserQuizId,
                        principalTable: "UserQuiz",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Questionid",
                table: "Answers",
                column: "Questionid");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_yearid",
                table: "Classes",
                column: "yearid");

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_Assighnmentid",
                table: "Lecture",
                column: "Assighnmentid");

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_Classid",
                table: "Lecture",
                column: "Classid");

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_Quizid",
                table: "Lecture",
                column: "Quizid");

            migrationBuilder.CreateIndex(
                name: "IX_LectureCode_Lectureid",
                table: "LectureCode",
                column: "Lectureid");

            migrationBuilder.CreateIndex(
                name: "IX_LectureCode_StudentId",
                table: "LectureCode",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Classid",
                table: "Notifications",
                column: "Classid");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_RightAnswerid",
                table: "Questions",
                column: "RightAnswerid",
                unique: true,
                filter: "[RightAnswerid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_Yearid",
                table: "User",
                column: "Yearid");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_Answerid",
                table: "UserAnswer",
                column: "Answerid");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_QuestionId",
                table: "UserAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_UserID",
                table: "UserAnswer",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_UserQuizId",
                table: "UserAnswer",
                column: "UserQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssighment_Assighmentid",
                table: "UserAssighment",
                column: "Assighmentid");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssighment_Studentid",
                table: "UserAssighment",
                column: "Studentid");

            migrationBuilder.CreateIndex(
                name: "IX_UserClasses_ClassId",
                table: "UserClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClassRequists_Classid",
                table: "UserClassRequists",
                column: "Classid");

            migrationBuilder.CreateIndex(
                name: "IX_UserClassRequists_Userid",
                table: "UserClassRequists",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_UserLecture_Lectureid",
                table: "UserLecture",
                column: "Lectureid");

            migrationBuilder.CreateIndex(
                name: "IX_UserLecture_StudentId",
                table: "UserLecture",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuiz_Quizid",
                table: "UserQuiz",
                column: "Quizid");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuiz_Studentid",
                table: "UserQuiz",
                column: "Studentid");

            migrationBuilder.CreateIndex(
                name: "IX_videoParts_Leactureid",
                table: "videoParts",
                column: "Leactureid");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions",
                table: "Answers",
                column: "Questionid",
                principalTable: "Questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions",
                table: "Answers");

            migrationBuilder.DropTable(
                name: "LectureCode");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "UserAnswer");

            migrationBuilder.DropTable(
                name: "UserAssighment");

            migrationBuilder.DropTable(
                name: "UserClasses");

            migrationBuilder.DropTable(
                name: "UserClassRequists");

            migrationBuilder.DropTable(
                name: "UserLecture");

            migrationBuilder.DropTable(
                name: "videoParts");

            migrationBuilder.DropTable(
                name: "UserQuiz");

            migrationBuilder.DropTable(
                name: "Lecture");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Assighment");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Year");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Quizes");
        }
    }
}
