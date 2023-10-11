using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kviz.Migrations
{
    /// <inheritdoc />
    public partial class History : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nickname = table.Column<string>(type: "TEXT", nullable: false),
                    Session_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Question_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Answer_Id = table.Column<int>(type: "INTEGER", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(name: "FK_Session_Id",
                    column: x => x.Session_Id,
                    principalTable: "Sessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "FK_Question_Id",
                    column: x => x.Question_Id,
                    principalTable: "Questions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "FK_Answer_Id",
                    column: x => x.Answer_Id,
                    principalTable: "Answers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");
        }
    }
}
