using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.DAL.Migrations
{
    /// <inheritdoc />
    public partial class adjustparents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentWithChildren");

            migrationBuilder.CreateTable(
                name: "ParentsWithChildren",
                columns: table => new
                {
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChildId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentsWithChildren", x => new { x.ParentId, x.ChildId });
                    table.ForeignKey(
                        name: "FK_ParentWithChildren_Child",
                        column: x => x.ChildId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ParentWithChildren_Parent",
                        column: x => x.ParentId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentsWithChildren_ChildId",
                table: "ParentsWithChildren",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsWithChildren_ParentId",
                table: "ParentsWithChildren",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentsWithChildren");

            migrationBuilder.CreateTable(
                name: "ParentWithChildren",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentWithChildren", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentWithChildren_User_ChildId",
                        column: x => x.ChildId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentWithChildren_User_ParentId",
                        column: x => x.ParentId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentWithChildren_ChildId",
                table: "ParentWithChildren",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentWithChildren_ParentId",
                table: "ParentWithChildren",
                column: "ParentId");
        }
    }
}
